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

    private ICurve NextCurve(ICurve inCurve, RotationDirection contoursRd) {
        var points = _pointsMap[inCurve.PointTo];
        if (points == null || points.Count == 0) 
            throw new Exception("ContoursWalker: NextCurve");
        
        if(points.Count==1)
            return points[0].OutCurve;

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
    }
}