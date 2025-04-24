
using PcbReader.Layers.Common;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Svg;
using PcbReader.Layers.Svg.Entities;
using SvgArcPart = PcbReader.Layers.Svg.Entities.ArcPathPart;
using GerberArcPart = PcbReader.Layers.Gerber.Entities.ArcPathPart;
using GerberLinePart = PcbReader.Layers.Gerber.Entities.LinePathPart;
using SvgLinePart = PcbReader.Layers.Svg.Entities.LinePathPart;
using SvgPath = PcbReader.Layers.Svg.Entities.Path;

namespace PcbReader.Converters.GerberToSvg;

public class GerberToSvgConverter {
    public SvgLayer Convert(GerberLayer layer) {
        var result = new SvgLayer();
        foreach(var operation in layer.Operations)
        {
            switch (operation) {
                case PathPaintOperation path:
                    break;
                case FlashOperation flash:
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: Convert");
            }
        }
        return result;
    }
    
    static SvgPath ConvertPath(PathPaintOperation operation) {
        var result = new SvgPath();
        result.StartPoint = operation.StartPoint;
        foreach (var op in operation.Parts) {
            switch (op) {
                case GerberLinePart line:
                    break;
                case GerberArcPart arc:
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: ConvertPath");
            }
        }
        return result;
    }




    
    static List<SvgArcPart> ConvertArcPath(Point gsp, GerberArcPart gap) {
        var result = new List<SvgArcPart>();
        var cx = gsp.X + gap.IOffset;
        var cy = gsp.Y + gap.JOffset;

        var r1 = Math.Sqrt(
            Math.Pow((double)cx - (double)gsp.X, 2) +
            Math.Pow((double)cy - (double)gsp.Y, 2));
        var r2 = Math.Sqrt(
            Math.Pow((double)cx - (double)gap.EndPoint.X, 2) +
            Math.Pow((double)cy - (double)gap.EndPoint.Y, 2));
        var tr = (decimal)(r2 + r1) / 2; //true radius

        if (gsp == gap.EndPoint) {

            var mpx = cx + (cx - gsp.X);
            var mpy = cy + (cy - gsp.Y);
            
            var part1 = new SvgArcPart {
                RotationDirection = gap.RotationDirection,
                Radius = tr,
                IsLargeArc = false,
                EndPoint = new Point(mpx, mpy)
            };

            var part2 = new SvgArcPart {
                RotationDirection = gap.RotationDirection,
                Radius = tr,
                IsLargeArc = true,
                EndPoint = gsp
            };
            result.Add(part1);
            result.Add(part2);
        } else {
            //var a = CalculateAngel(gsp, gap.EndPoint, new Point())
            var part = new SvgArcPart {
                RotationDirection = gap.RotationDirection,
                EndPoint = gap.EndPoint,
                Radius = tr
            };
        }
        return result;
    }
}

