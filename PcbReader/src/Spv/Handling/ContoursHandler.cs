using PcbReader.Spv.Entities;
using PcbReader.Spv.Entities.GraphicElements;
using PcbReader.Spv.Entities.GraphicElements.Curves;
using PcbReader.Spv.Relations;

namespace PcbReader.Spv.Handling;

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

    static ICurve MergeWalk(TransitionPoint tp, ICurve prevCurve, RotationDirection rd) {

        return null;
    }
    

}