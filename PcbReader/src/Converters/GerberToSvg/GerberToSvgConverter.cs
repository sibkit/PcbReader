
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

public static class GerberToSvgConverter {
    public static SvgLayer Convert(GerberLayer layer) {
        var result = new SvgLayer();
        foreach(var operation in layer.Operations)
        {
            switch (operation) {
                case PathPaintOperation path:
                    result.Paths.Add(ConvertPath(path));
                    break;
                case FlashOperation flash:
                    Console.WriteLine("Flash not implemented");
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: Convert");
            }
        }
        return result;
    }
    
    static SvgPath ConvertPath(PathPaintOperation operation) {
        var result = new SvgPath {
            StartPoint = operation.StartPoint
        };
        var startPartPoint = operation.StartPoint;
        foreach (var op in operation.Parts) {
            switch (op) {
                case GerberLinePart line:
                    result.Parts.Add(new SvgLinePart {
                        EndPoint = line.EndPoint
                    });
                    break;
                case GerberArcPart arc:
                    result.Parts.AddRange(ConvertArcPath(startPartPoint, arc));
                    startPartPoint = arc.EndPoint;
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: ConvertPath");
            }
        }
        result.IsClosed = operation.IsClosed;
        return result;
    }
    
    static List<SvgArcPart> ConvertArcPath(Point gsp, GerberArcPart gap) {
        var result = new List<SvgArcPart>();
        var cx = gsp.X + gap.IOffset;
        var cy = gsp.Y + gap.JOffset;

        var r1 = Math.Sqrt(
            Math.Pow(cx - gsp.X, 2) +
            Math.Pow(cy - gsp.Y, 2));
        var r2 = Math.Sqrt(
            Math.Pow(cx - gap.EndPoint.X, 2) +
            Math.Pow(cy - gap.EndPoint.Y, 2));
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
            var arcWay = Geometry.ArcWay(gsp, gap.EndPoint, new Point(cx, cy));
            
            var part = new SvgArcPart {
                RotationDirection = arcWay.RotationDirection,
                EndPoint = gap.EndPoint,
                Radius = tr,
                IsLargeArc = arcWay.IsLarge,
            };
            result.Add(part);
            
        }
        return result;
    }
}

