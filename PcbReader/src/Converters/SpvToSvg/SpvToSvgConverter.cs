
using PcbReader.Layers.Svg.Entities;
using PcbReader.Spv.Entities;
using PcbReader.Spv.Entities.GraphicElements;
using PcbReader.Spv.Entities.GraphicElements.Curves;

namespace PcbReader.Converters.SpvToSvg;

public  static class SpvToSvgConverter {
    public static SvgLayer Convert(Area area) {
        var result = new SvgLayer();
        result.Elements.AddRange(area.GraphicElements);
        //InvertAxis(result);
        return result;
    }
    
    static void InvertAxis(SvgLayer layer) {
        foreach (var e in layer.Elements) {
            switch (e) {
    
                case CurvesOwner ctr:
                    InvertAxis(ctr);
                    break;
                case Shape shape:
                    InvertAxis(shape);
                    break;
                case Dot dot:
                    dot.CenterPoint = new Point(dot.CenterPoint.X, -dot.CenterPoint.Y);
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: InvertAxis");
            }
        }
    }
    
    static ICurve InvertAxis(ICurve pathPart) {
        return pathPart switch {
            Line line => new Line {
                PointTo = line.PointTo.WithNewY(-line.PointTo.Y), 
                PointFrom = line.PointFrom.WithNewY(-line.PointFrom.Y),
            },
            Arc arc => new Arc {
                PointTo = arc.PointTo.WithNewY(-arc.PointTo.Y),
                PointFrom = arc.PointFrom.WithNewY(-arc.PointFrom.Y),
                IsLargeArc = arc.IsLargeArc,
                RotationDirection = arc.RotationDirection.Invert(),
                Radius = arc.Radius,
            },
            _ => throw new Exception("GerberToSvgConverter: InvertAxis")
        };
    }
    
    
    static void InvertAxis(Shape shape) {
        foreach (var p in shape.OuterContours) {
            InvertAxis(p);
        }
        foreach (var p in shape.InnerContours) {
            InvertAxis(p);
        }
    }
    static void InvertAxis(CurvesOwner ctx) {
        foreach (var p in ctx.Curves) {
            InvertAxis(p);
        }
    }
}