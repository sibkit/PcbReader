using PcbReader.Core.GraphicElements;
using PcbReader.Core.GraphicElements.PathParts;

namespace PcbReader.Core;

public static class Contours {


    private static Contour Simplify(Contour contour) {

        var result = new Contour();

        for (var i = 0; i < contour.Parts.Count; i++) {
            switch (contour.Parts[i]) {
                case LinePathPart lpp:
                    result.Parts.Add((LinePathPart)lpp.Clone());
                    break;
            }
        }
        return result;
    }
    
    public static RotationDirection GetRotationDirection(Contour contour) {
        contour = Simplify(contour);
        if(contour.Parts.Count<2)
            throw new ApplicationException("Контур состоит менее чем из 2-х частей");
        foreach (IPathPart pp in contour.Parts) {
            
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