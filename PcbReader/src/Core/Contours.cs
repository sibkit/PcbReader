using PcbReader.Core.GraphicElements;

namespace PcbReader.Core;

public static class Contours {
    public static RotationDirection GetRotationDirection(Contour contour) {
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