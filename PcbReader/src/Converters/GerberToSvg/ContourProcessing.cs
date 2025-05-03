using PcbReader.Geometry;
using PcbReader.Geometry.PathParts;
using PcbReader.Layers.Gerber.Entities.Apertures.Macro;
using PcbReader.Layers.Svg.Entities;
using Path = PcbReader.Geometry.Path;

namespace PcbReader.Converters.GerberToSvg;

public static class ContourProcessing {
    // public static Path CombinePaths(Path path1, Path path2) {
    //     foreach (IPathPart part in path1.Parts) {
    //         switch (part) {
    //             case LinePathPart:
    //                 break;
    //             case ArcPathPart:
    //                 break;
    //         }
    //     }
    // }
    public static Shape Subtract(Shape minuend, Shape subtrahend) {
        var result = new Shape();
        return result;
    }


}