using PcbReader.Layers.Svg.Entities;
using PcbReader.Spv.Entities;

namespace PcbReader.Converters.SpvToSvg;

public  static class SpvToSvgConverter {
    public static SvgLayer Convert(Area area) {
        var result = new SvgLayer();
        result.Elements.AddRange(area.GraphicElements);
        //InvertAxis(result);
        return result;
    }
    
    // static void InvertAxis(SvgLayer layer) {
    //     foreach (var e in layer.Elements) {
    //         switch (e) {
    //
    //             case PathPartsOwner ctr:
    //                 InvertAxis(ctr);
    //                 break;
    //             case Shape shape:
    //                 InvertAxis(shape.OuterContour);
    //                 foreach (var ic in shape.InnerContours)
    //                     InvertAxis(ic);
    //                 break;
    //             case Dot dot:
    //                 dot.CenterPoint = new Point(dot.CenterPoint.X, -dot.CenterPoint.Y);
    //                 break;
    //             default:
    //                 throw new Exception("GerberToSvgConverter: InvertAxis");
    //         }
    //     }
    // }
    //
    // static IPathPart InvertAxis(IPathPart pathPart) {
    //     return pathPart switch {
    //         LinePathPart line => new LinePathPart {
    //             PointTo = line.PointTo.WithNewY(-line.PointTo.Y), 
    //             PointFrom = line.PointFrom.WithNewY(-line.PointFrom.Y),
    //         },
    //         ArcPathPart arc => new ArcPathPart {
    //             PointTo = arc.PointTo.WithNewY(-arc.PointTo.Y),
    //             PointFrom = arc.PointFrom.WithNewY(-arc.PointFrom.Y),
    //             IsLargeArc = arc.IsLargeArc,
    //             RotationDirection = arc.RotationDirection.Invert(),
    //             Radius = arc.Radius,
    //         },
    //         _ => throw new Exception("GerberToSvgConverter: InvertAxis")
    //     };
    // }
    //
    // static void InvertAxis(PathPartsOwner ctx) {
    //     //ctx.StartPoint = ctx.StartPoint.WithNewY(-ctx.StartPoint.Y);
    //     foreach (var p in ctx.Parts) {
    //         InvertAxis(p);
    //     }
    // }
}