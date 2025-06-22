using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements;
using PcbReader.Core.Entities.GraphicElements.Curves;

namespace PcbReader.Core.Handling;

public class ContoursWalker {
    
    private readonly Dictionary<Point, List<TransitionPoint>> _pointsMap = new ();
    
    public Contour Contour1 { get; init; }
    public Contour Contour2 { get; init; }
    
    private void FillPointsMap(Contour contour) {
        for (var i = 0; i < contour.Curves.Count; i++) {
            var outCurve = contour.Curves[i];
            var inCurve = i > 0 ? contour.Curves[i-1]:contour.Curves.Last();
            var rp = Geometry.RoundPoint(outCurve.PointFrom);
            if(!_pointsMap.ContainsKey(rp))
                _pointsMap.Add(rp, []);
            _pointsMap[rp].Add(new TransitionPoint {
                Contour = contour,
                InCurve = inCurve,
                OutCurve = outCurve
            });
        }
    }

    // private static (double inA, double outA) CalculateAngles(TransitionPoint point) {
    //     return (
    //         Angles.PositiveNormalize(Angles.CalculateAngle(point.InCurve.PointTo.WithNewX(point.InCurve.PointTo.X + 1), point.InCurve.PointTo, point.InCurve.PointFrom)),
    //         Angles.PositiveNormalize(Angles.CalculateAngle(point.OutCurve.PointTo.WithNewX(point.OutCurve.PointTo.X + 1), point.OutCurve.PointTo, point.OutCurve.PointFrom))
    //     );
    // }

    private ICurve FindFirstCurve() {
        Point? firstPoint = null;
        var minX = double.PositiveInfinity;
        foreach (var kvp in _pointsMap) {
            if (kvp.Key.X < minX) {
                minX = kvp.Key.X;
                firstPoint = kvp.Key;
            }
        }

        if (firstPoint == null)
            throw new Exception("ContoursWalker: WalkMerge");

        var points = _pointsMap[firstPoint.Value];
        var angle = double.PositiveInfinity;
        ICurve result = null;
        foreach (var tp in points) {
            var a = Angles.PositiveNormalize(Angles.CalculateAngle(firstPoint.Value.WithNewY(firstPoint.Value.Y-1), tp.InCurve.PointFrom, firstPoint.Value));
            if (a < angle) {
                angle = a;
                result = tp.InCurve;
            }
        }
        return result;
    }
    
    // private Point GetFirstPoint() {
    //     Point? firstPoint = null;
    //     var minX = double.PositiveInfinity;
    //     foreach (var kvp in _pointsMap) {
    //         if (kvp.Key.X < minX) {
    //             minX = kvp.Key.X;
    //             firstPoint = kvp.Key;
    //         }
    //     }
    //
    //     if (firstPoint == null)
    //         throw new Exception("ContoursWalker: WalkMerge");
    //     return firstPoint.Value;
    // }

    private ICurve NextCurve(ICurve inCurve, RotationDirection contoursRd) {
        var points = _pointsMap[inCurve.PointTo];
        if (points == null || points.Count == 0) 
            throw new Exception("ContoursWalker: NextCurve");
        
        if(points.Count==1)
            return points[0].OutCurve;

        
        //var outCurves = points.Select(point => point.OutCurve).ToList();

        var angle = double.PositiveInfinity;
        ICurve result = null;
        foreach (var tp in points) {
            var a = contoursRd == RotationDirection.CounterClockwise ?
                Angles.PositiveNormalize(Angles.CalculateAngle(inCurve.PointFrom, tp.OutCurve.PointTo, inCurve.PointTo)) :
                Angles.PositiveNormalize(Angles.CalculateAngle(tp.OutCurve.PointTo, inCurve.PointFrom, inCurve.PointTo));
            if (a < angle) {
                angle = a;
                result = tp.OutCurve;
            }
        }
        return result;

        // var tuples = new List<(TransitionPoint tp, ICurve curve)>();
        //
        // foreach (var point in points) {
        //     tuples.Add((point, point.InCurve));
        //     tuples.Add((point, point.OutCurve));
        // }
        //
        // tuples.Sort((l1, l2) => {
        //     var vec1 = new Vector(l1.curve.PointTo.X - l1.curve.PointFrom.X, l1.curve.PointTo.Y - l1.curve.PointFrom.Y);
        //     var vec2 = new Vector(l2.curve.PointTo.X - l2.curve.PointFrom.X, l2.curve.PointTo.Y - l2.curve.PointFrom.Y);
        //     return Vectors.CrossProduct(vec1, vec2) switch {
        //         0 => 0,
        //         < 0 => -1,
        //         _ => 1
        //     };
        // });
        //
        // return tuples[0].tp.OutCurve;
        //
        // var curVal = 0;
        // var maxVal = int.MinValue;
        // TransitionPoint resultTp = null;
        //
        // foreach (var tuple in tuples) {
        //     if (tuple.isIn) {
        //         curVal = curVal - 1;
        //     } else {
        //         curVal = curVal + 1;
        //         if (curVal > maxVal) {
        //             maxVal = curVal;
        //             resultTp = tuple.tp;
        //         }
        //     }
        // }
        //
        // return resultTp!.OutCurve;
        //
        // (double angle, TransitionPoint tp)? pIn = null;
        // (double angle, TransitionPoint tp)? pOut = null;
        //
        // foreach (var tuple in tuples) {
        //     switch (tuple.isIn) {
        //         case true when pIn == null:
        //             pIn = (tuple.angle, tuple.tp);
        //             break;
        //         case false:
        //             pOut = (tuple.angle, tuple.tp);
        //             break;
        //     }
        // }
        //
        // return pOut?.tp.OutCurve;
    }
    
    public Contour WalkMerge() {
        if (Contour1 == Contour2)
            return null;
        if(Contour1 == null || Contour2 == null)
            return null;
        if (!Contour1.Bounds.IsIntersected(Contour2.Bounds))
            return null;
        
        var sc1 = ContoursHandler.SplitByRelationPoints(Contour1, Contour2);
        var sc2 = ContoursHandler.SplitByRelationPoints(Contour2, Contour1);
       
        if (Contours.GetRotationDirection(sc1) != Contours.GetRotationDirection(sc2))
            sc2 = Contours.GetReversed(sc2);

        _pointsMap.Clear();
        FillPointsMap(sc1);
        FillPointsMap(sc2);
        
        var rd = Contours.GetRotationDirection(Contour1);
        // var m = rd switch {
        //     RotationDirection.Clockwise => 1d,
        //     RotationDirection.CounterClockwise => -1d,
        //     _ => throw new Exception("Invalid rotation direction " + rd)
        // };

        var result = new Contour();

        var firstCurve = FindFirstCurve();
        var curCurve = firstCurve;
        while (true) {
            curCurve = NextCurve(curCurve, rd);
            result.Curves.Add(curCurve);
            if(curCurve == firstCurve)
                break;
        }

        return result;
        // var curCurve = firstPoint.InCurve;
        // var curTp = _pointsMap[firstPoint];
        //
        // while (true) {
        //     var pointsSet = _pointsMap[Geometry.RoundPoint(curCurve.PointTo)];
        //     if (pointsSet == null || pointsSet.Count == 0)
        //         throw new Exception("ContoursWalker: WalkMerge");
        //     if (pointsSet.Count == 1) {
        //         result.Curves.Add(pointsSet[0].OutCurve);
        //         curCurve = pointsSet[0].OutCurve;
        //     } else {
        //         TransitionPoint nextTp = null;
        //         var nearestAngle = rd == RotationDirection.Clockwise? 2*Math.PI : -2*Math.PI;
        //         foreach (var tp in pointsSet) {
        //             var v1 = new Vector(curCurve.PointTo.X-curCurve.PointFrom.X, curCurve.PointTo.Y-curCurve.PointFrom.Y);
        //             var v2 = new Vector(tp.OutCurve.PointTo.X-tp.OutCurve.PointFrom.X,tp.OutCurve.PointTo.Y-tp.OutCurve.PointFrom.Y);
        //             var angle = Angles.PiNormalize(Vectors.Angle(v1, v2));
        //             if (rd == RotationDirection.Clockwise && angle < nearestAngle || rd == RotationDirection.CounterClockwise && angle > nearestAngle) {
        //                 nearestAngle = angle;
        //                 nextTp = tp;
        //             }
        //         }
        //
        //         if (nextTp == null) {
        //             throw new Exception("ContoursWalker: WalkMerge");
        //         }
        //         result.Curves.Add(nextTp.OutCurve);
        //             curCurve = nextTp.OutCurve;
        //     }
        //     if(curCurve == firstPoint.InCurve)
        //         break;
        // }
        //
        //
        //
        //
        // return result;
    }
}