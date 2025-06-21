using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements;
using PcbReader.Core.Entities.GraphicElements.Curves;
using PcbReader.Core.Relations;

namespace PcbReader.Core.Handling;

public static class ContoursHandler {
    
      public static Contour SplitByRelationPoints(Contour contour1, Contour contour2) {
        if (!contour1.Bounds.IsIntersected(contour2.Bounds))
            return contour1;
        
        var result = new Contour();
        
        //находим все пересечения
        foreach (var curve in contour1.Curves) {
            var contactPoints = new List<ContactPoint>();
            foreach (var curve2 in contour2.Curves) {
                var relation = RelationManager.DefineRelation(curve, curve2);
                switch (relation) {
                    case NotRelation:
                        break;
                    case ContactRelation ir:
                        contactPoints.AddRange(ir.Points);
                        break;
                    case OverlappingRelation or:
                        contactPoints.AddRange(or.Points);
                        break;
                    default:
                        throw new Exception("Contours: SplitByRelationPoints (Unknown Relation type: " + relation+")");
                }
            }
            
            contactPoints.Sort((x,x1)=>x.T.CompareTo(x1.T));
            switch (curve) {
                case Line line:
                    var sp = line.PointFrom;
                    foreach (var cp in contactPoints) {
                        result.Curves.Add(new Line {
                            PointFrom = sp,
                            PointTo = cp.Point,
                        });
                        sp = cp.Point;
                    }
                    result.Curves.Add(new Line {
                        PointFrom = sp,
                        PointTo = line.PointTo,
                    });
                    break;
                case Arc arc:
                    var sp2 = arc.PointFrom;
                    foreach (var cp in contactPoints) {
                        result.Curves.Add(new Arc {
                            PointFrom = sp2,
                            PointTo = cp.Point,
                            Radius = arc.Radius,
                            RotationDirection = arc.RotationDirection,
                            IsLargeArc = arc.IsLargeArc
                        });
                        sp2 = cp.Point;
                    }
                    result.Curves.Add(new Arc {
                        PointFrom = sp2,
                        PointTo = arc.PointTo,
                        Radius = arc.Radius,
                        RotationDirection = arc.RotationDirection,
                        IsLargeArc = arc.IsLargeArc
                    });
                    break;
                default:
                    throw new Exception("Contours: Merge (Unknown Curve type: " + curve + ")");
            }
        }

        return result;
    }


    public static Point RoundPoint(Point point) {
        return new Point(Math.Round(point.X, 10), Math.Round(point.Y, 10));
    }

    private static void FillPointsMap(Contour contour, Dictionary<Point, List<TransitionPoint>> pointsMap) {
        for (var i = 0; i < contour.Curves.Count; i++) {
            var outCurve = contour.Curves[i];
            var inCurve = i > 0 ? contour.Curves[i]:contour.Curves.Last();
            var rp = RoundPoint(outCurve.PointFrom);
            if(!pointsMap.ContainsKey(rp))
                pointsMap.Add(rp, []);
            pointsMap[rp].Add(new TransitionPoint {
                Contour = contour,
                InCurve = inCurve,
                OutCurve = outCurve
            });
        }
    }
    
    public static Dictionary<Point, List<TransitionPoint>> GetPointsMap(Contour contour1, Contour contour2) {

        var c1 = SplitByRelationPoints(contour1, contour2);
        if (c1 == contour1)
            return null;
        var c2 = SplitByRelationPoints(contour2, contour1);
        
        var result = new Dictionary<Point, List<TransitionPoint>>();
        FillPointsMap(c1, result);
        FillPointsMap(c2, result);
        return result;
    }
}