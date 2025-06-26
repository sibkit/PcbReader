using PcbReader.Spv.Entities;
using PcbReader.Spv.Entities.GraphicElements;
using PcbReader.Spv.Entities.GraphicElements.Curves;
using PcbReader.Spv.Relations;

namespace PcbReader.Spv.Handling;

public static class Contours {
    private static Contour Simplify(Contour contour) {

        var result = new Contour();

        for (var i = 0; i < contour.Curves.Count; i++) {
            switch (contour.Curves[i]) {
                case Line lpp:
                    result.Curves.Add((Line)lpp.Clone());
                    break;
                case Arc arc:
                    var mp = Geometry.ArcMiddlePoint(arc);
                    result.Curves.Add(new Line {
                        PointFrom = arc.PointFrom,
                        PointTo = mp,
                    });
                    result.Curves.Add(new Line {
                        PointFrom = mp,
                        PointTo = arc.PointTo,
                    });
                    break;
            }
        }

        return result;
    }

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
                        throw new Exception("Contours: SplitByRelationPoints (Unknown Relation type: " + relation + ")");
                }
            }

            contactPoints.Sort((x, x1) => x.T.CompareTo(x1.T));
            switch (curve) {
                case Line line:
                    var sp = line.PointFrom;
                    foreach (var cp in contactPoints) {
                        if (sp != cp.Point) {
                            result.Curves.Add(new Line {
                                PointFrom = sp,
                                PointTo = cp.Point,
                            });
                            sp = cp.Point;
                        }
                    }

                    if (sp != line.PointTo)
                        result.Curves.Add(new Line {
                            PointFrom = sp,
                            PointTo = line.PointTo,
                        });
                    break;
                case Arc arc:
                    var sp2 = arc.PointFrom;
                    foreach (var cp in contactPoints) {
                        if (sp2 != cp.Point) {
                            result.Curves.Add(new Arc {
                                PointFrom = sp2,
                                PointTo = cp.Point,
                                Radius = arc.Radius,
                                RotationDirection = arc.RotationDirection,
                                IsLargeArc = arc.IsLargeArc
                            });
                        }
                        sp2 = cp.Point;
                    }

                    if (sp2 != arc.PointTo) {
                        result.Curves.Add(new Arc {
                            PointFrom = sp2,
                            PointTo = arc.PointTo,
                            Radius = arc.Radius,
                            RotationDirection = arc.RotationDirection,
                            IsLargeArc = arc.IsLargeArc
                        });
                    }

                    break;
                default:
                    throw new Exception("Contours: Merge (Unknown Curve type: " + curve + ")");
            }
        }

        return result;
    }
    
    private static double GetRotationAngle(Line curLine, Line prevLine) {
        var th1 = Math.Atan2(prevLine.PointTo.Y - prevLine.PointFrom.Y, prevLine.PointTo.X - prevLine.PointFrom.X);
        var th2 = Math.Atan2(curLine.PointTo.Y - curLine.PointFrom.Y, curLine.PointTo.X - curLine.PointFrom.X);
        return Angles.PiNormalize(th1 - th2);
    }

    public static RotationDirection GetRotationDirection(Contour contour) {
        contour = Simplify(contour);
        if (contour.Curves.Count < 2)
            throw new ApplicationException("Контур состоит менее чем из 2-х частей");
        var resultAngle = 0d;
        var prevLine = (Line)contour.Curves[^1];
        foreach (var curve in contour.Curves) {
            switch (curve) {
                case Line line:
                    var ra = GetRotationAngle(line, prevLine);
                    resultAngle += ra;
                    prevLine = line;
                    break;
                default:
                    throw new Exception("Contours: GetRotationDirection(1)");
            }
        }


        
        if (Math.Abs(resultAngle - Math.PI * 2) < Geometry.Accuracy)
            return RotationDirection.Clockwise;
        if (Math.Abs(resultAngle + Math.PI * 2) < Geometry.Accuracy)
            return RotationDirection.CounterClockwise;
        if (Math.Abs(resultAngle) < Geometry.Accuracy)
            return RotationDirection.None;
        throw new Exception("Contours: GetRotationDirection(2)");
    }

    public static Contour GetReversed(Contour contour) {
        var result = new Contour();
        for (var i = contour.Curves.Count - 1; i >= 0; i--) {
            var part = contour.Curves[i];
            result.Curves.Add(part.GetReversed());
        }

        return result;
    }


    public static Shape Union(Contour contour1, Contour contour2) {
        var cw = new ContoursWalker(contour1, contour2);
        return cw.WalkMerge();
    }

    public static Shape Subtract(Contour contour1, Contour contour2) {
        return null;
    }
    
    public static Shape Intersect(Contour contour1, Contour contour2) {
        return null;
    }
    
    public static Shape Exclude(Contour contour1, Contour contour2) {
        return null;
    }
    
    // public static ContactPoint FindExtremePoint(Line line, Contour contour) {
    //     var contactPoints = new List<ContactPoint>();
    //     foreach (var curve2 in contour.Curves) {
    //         var relation = RelationManager.DefineRelation(line, curve2);
    //         switch (relation) {
    //             case NotRelation:
    //                 break;
    //             case ContactRelation ir:
    //                 contactPoints.AddRange(ir.Points);
    //                 break;
    //             case OverlappingRelation or:
    //                 contactPoints.AddRange(or.Points);
    //                 break;
    //             default:
    //                 throw new Exception("Contours: SplitByRelationPoints (Unknown Relation type: " + relation + ")");
    //         }
    //     }
    //
    //     if (contactPoints.Count == 0)
    //         return null;
    //     contactPoints.Sort((a, b) => a.T.CompareTo(b.T));
    //     return contactPoints[^1];
    // }

  

    

}

