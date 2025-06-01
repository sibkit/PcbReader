using PcbReader.Core.GraphicElements;
using PcbReader.Core.GraphicElements.PathParts;

namespace PcbReader.Core;

public static class Contours {
    
    private static Contour Simplify(Contour contour) {

        var result = new Contour();

        for (var i = 0; i < contour.Parts.Count; i++) {
            switch (contour.Parts[i]) {
                case Line lpp:
                    result.Parts.Add((Line)lpp.Clone());
                    break;
                case Arc arc:
                    var mp = Geometry.ArcMiddlePoint(arc);
                    result.Parts.Add(new Line {
                        PointFrom = arc.PointFrom,
                        PointTo = mp,
                    });
                    result.Parts.Add(new Line {
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
        if (contour.Parts.Count < 2)
            throw new ApplicationException("Контур состоит менее чем из 2-х частей");
        var resultAngle = 0d;
        var prevLine = (Line)contour.Parts[^1];
        foreach (var curve in contour.Parts) {
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
        for (var i = contour.Parts.Count - 1; i >= 0; i--) {
            var part = contour.Parts[i];
            result.Parts.Add(part.GetReversed());
        }
        return result;
    }
}