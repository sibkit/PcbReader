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
                case Arc app:
                    
                    break;
            }
        }
        return result;
    }


    // private static double GetRotationAngle(IPathPart pp) {
    //     switch (pp) {
    //         case LinePathPart lpp:
    //             
    //             break;
    //         case ArcPathPart app:
    //             var cp = Geometry.ArcCenter(app);
    //             var angle = Geometry.CalculateAngle(app.PointFrom, app.PointTo, cp);
    //             return app.RotationDirection switch {
    //                 RotationDirection.ClockWise => angle * (-1),
    //                 RotationDirection.CounterClockwise => angle,
    //                 _ => throw new Exception("Contours: GetRotationAngle(1)")
    //             };
    //         default:
    //             throw new Exception("Contours: GetRotationAngle(2)");
    //     }
    // }
    
    public static RotationDirection GetRotationDirection(Contour contour) {
        //contour = Simplify(contour);
        if(contour.Parts.Count<2)
            throw new ApplicationException("Контур состоит менее чем из 2-х частей");
        foreach (var pp in contour.Parts) {

        }

        throw new NotImplementedException();
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