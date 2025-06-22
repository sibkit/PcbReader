using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements;
using PcbReader.Core.Entities.GraphicElements.Curves;

namespace PcbReader.Core.Handling;

public class ContoursWalker {
    
    private readonly Dictionary<Point, List<TransitionPoint>> _pointsMap = new ();
    private readonly List<ICurve> _placedCurves = [];
    
    public Contour Contour1 { get; }
    public Contour Contour2 { get; }

    public ContoursWalker(Contour contour1, Contour contour2) {

        
        // if (Contour1 == Contour2)
        //     return null;
        // if(Contour1 == null || Contour2 == null)
        //     return null;
        // if (!Contour1.Bounds.IsIntersected(Contour2.Bounds))
        //     return null;
        
        Contour1 = ContoursHandler.SplitByRelationPoints(contour1, contour2);
        Contour2 = ContoursHandler.SplitByRelationPoints(contour2, contour1);
       
        if (Contours.GetRotationDirection(Contour1) != Contours.GetRotationDirection(Contour2))
            Contour2 = Contours.GetReversed(Contour2);
        
        FillPointsMap(Contour1);
        FillPointsMap(Contour2);
    }
    
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
            if(_placedCurves.Contains(tp.OutCurve))
                continue;
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


    public List<Contour> Walk() {
        List<Contour> result = [];
        foreach (var startCurve in Contour1.Curves.Union(Contour2.Curves)) {
            if (_placedCurves.Contains(startCurve))
                continue;
            var visitedCurves = new List<ICurve>();
            var prevCurve = startCurve;
            ICurve firstCurve = null;
            while (true) {
                visitedCurves.Add(prevCurve);
                var curve = NextCurve(prevCurve, RotationDirection.Clockwise);
                if(_placedCurves.Contains(curve))
                    break;
                if (visitedCurves.Contains(curve)) {
                    firstCurve = curve;
                    break;
                }
                prevCurve = curve;
            }

            if (firstCurve != null) {
                var c = new Contour();
                c.Curves.AddRange(visitedCurves.Skip(visitedCurves.IndexOf(firstCurve)));
                _placedCurves.AddRange(c.Curves);
                result.Add(c);
            }
        }
        return result;
    }
    
    public Contour WalkMerge() {

        
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