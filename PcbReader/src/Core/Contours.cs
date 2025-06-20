﻿using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements;
using PcbReader.Core.Entities.GraphicElements.Curves;
using PcbReader.Core.Handling;
using PcbReader.Core.Relations;

namespace PcbReader.Core;

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
                    resultAngle += GetRotationAngle(line, prevLine);
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
    
    public static ContactPoint FindExtremePoint(Line line, Contour contour) {
        var contactPoints = new List<ContactPoint>();
        foreach (var curve2 in contour.Curves) {
            var relation = RelationManager.DefineRelation(line, curve2);
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

        if (contactPoints.Count == 0)
            return null;
        contactPoints.Sort((a, b) => a.T.CompareTo(b.T));
        return contactPoints[^1];
    }

  

    
    public static Shape Merge(Contour contour1, Contour contour2) {
        if (!contour1.Bounds.IsIntersected(contour2.Bounds))
            return null;

        var sp = contour1.Curves[0].PointFrom;
        var epX = contour1.Bounds.MinX<contour2.Bounds.MinX? contour1.Bounds.MinX : contour2.Bounds.MinX;
        var line = new Line {
            PointFrom = sp,
            PointTo = new Point(epX, sp.Y)
        };

        var sc1 = ContoursHandler.SplitByRelationPoints(contour1, contour2);
        var sc2 = ContoursHandler.SplitByRelationPoints(contour2, contour1);
       
        if (GetRotationDirection(sc1) != GetRotationDirection(sc2))
            sc2 = GetReversed(sc2);
        
        
        
        return new Shape {
            OuterContour = null
        };
    }
}

