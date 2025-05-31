using PcbReader.Core.GraphicElements;
using PcbReader.Core.GraphicElements.PathParts;

namespace PcbReader.Core;

public class Area {
    public List<IGraphicElement> GraphicElements { get; } = [];

    public void InvertYAxe() {
        foreach (var e in GraphicElements) {
            switch (e) {

                case PathPartsOwner ctr:
                    InvertYAxe(ctr);
                    break;
                case Shape shape:
                    InvertYAxe(shape.OuterContour);
                    foreach (var ic in shape.InnerContours)
                        InvertYAxe(ic);
                    break;
                case Dot dot:
                    dot.CenterPoint = new Point(dot.CenterPoint.X, -dot.CenterPoint.Y);
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: InvertAxis");
            }
            e.UpdateBounds();
        }
    }
    
    static void InvertYAxe(IPathPart pathPart) {

        switch (pathPart) {
            case LinePathPart line:
                line.PointFrom = new Point(line.PointFrom.X, -line.PointFrom.Y);
                line.PointTo = new Point(line.PointTo.X, -line.PointTo.Y);
                break;
            case ArcPathPart arc:
                arc.PointFrom = new Point(arc.PointFrom.X, -arc.PointFrom.Y);
                arc.PointTo = new Point(arc.PointTo.X, -arc.PointTo.Y);
               // arc.RotationDirection = arc.RotationDirection.Invert();
                break;
            default:
                throw new Exception("Area: InvertAxis");
        }
    }

    static void InvertYAxe(PathPartsOwner ctx) {
        //ctx.StartPoint = ctx.StartPoint.WithNewY(-ctx.StartPoint.Y);
        foreach (var p in ctx.Parts) {
            InvertYAxe(p);
        }
    }
    
}