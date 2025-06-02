using PcbReader.Core.GraphicElements;
using PcbReader.Core.Intersections;
using PcbReader.Core.GraphicElements.PathParts;

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
        else if (Math.Abs(resultAngle + Math.PI * 2) < Geometry.Accuracy)
            return RotationDirection.CounterClockwise;
        else
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

    public static Contour Merge(Contour contour1, Contour contour2) {
        if (!contour1.Bounds.IsIntersected(contour2.Bounds))
            return null;
        var result = new Contour();

        if (GetRotationDirection(contour1) != GetRotationDirection(contour2))
            contour2 = GetReversed(contour2);

        foreach (var curve in contour1.Curves) {
            var finded = false;
            foreach (var curve2 in contour2.Curves) {
                var intersections = Intersections.Intersections.FindIntersections(curve, curve2);
                if (intersections.Count != 0) {
                    finded = true;
                } else {
                    
                }
            }
            if(!finded)
                result.Curves.Add(curve);
        }
        
        
        
        return result;
    }
}