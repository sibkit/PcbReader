using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Svg.Entities;
using PcbReader.Spv;
using PcbReader.Spv.Entities;
using PcbReader.Spv.Entities.GraphicElements;
using PcbReader.Spv.Entities.GraphicElements.Curves;
using GerberArcPart = PcbReader.Layers.Gerber.Entities.ArcPathPart;
using GerberLinePart = PcbReader.Layers.Gerber.Entities.LinePathPart;
using Path = PcbReader.Spv.Entities.GraphicElements.Path;

namespace PcbReader.Converters.GerberToSpv;

public static class GerberToSpvConverter {
    
    public static SvgLayer Convert(GerberLayer layer) {

        var result = new SvgLayer();
        var apertureConverter = new ApertureConverter(layer);
        foreach(var operation in layer.Operations)
        {
            switch (operation) {
                case PathPaintOperation path:
                    result.Elements.Add(ConvertPath(path));
                    break;
                case FlashOperation flash:
                    var aperture = layer.Apertures[flash.ApertureCode];
                    result.Elements.AddRange(apertureConverter.ConvertAperture(flash.Point, aperture));
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: Convert");
            }
        }
        //InvertAxis(result);
        return result;
    }
    
    static Path ConvertPath(PathPaintOperation operation) {
        var result = new Path {
            StrokeWidth = operation.Aperture.Diameter,
            //StartPoint = operation.StartPoint,
        };

        var startPartPoint = operation.StartPoint;
        foreach (var op in operation.Parts) {
            switch (op) {
                case GerberLinePart line:
                    result.Curves.Add(new Line {
                        PointFrom = startPartPoint,
                        PointTo = line.EndPoint
                    });
                    break;
                case GerberArcPart arc:
                    result.Curves.AddRange(ConvertArcPath(startPartPoint, arc, result));
                    startPartPoint = arc.EndPoint;
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: ConvertPath");
            }
        }
        return result;
    }
    
    static List<Arc> ConvertArcPath(Point gsp, GerberArcPart gap, CurvesOwner owner) {
        var result = new List<Arc>();
        //var cx = gsp.X + gap.IOffset;
        //var cy = gsp.Y - gap.JOffset;
        var cx = gap.EndPoint.X < gsp.X ? gsp.X - gap.IOffset : gsp.X + gap.IOffset;
        var cy = gap.EndPoint.Y < gsp.Y ? gsp.Y - gap.JOffset : gsp.Y + gap.JOffset;
        
        var r1 = Math.Sqrt(
            Math.Pow(cx - gsp.X, 2) +
            Math.Pow(cy - gsp.Y, 2));
        var r2 = Math.Sqrt(
            Math.Pow(cx - gap.EndPoint.X, 2) +
            Math.Pow(cy - gap.EndPoint.Y, 2));
        var tr = (r2 + r1) / 2; //true radius
        var arcWay = Geometry.ArcWay(gsp, gap.EndPoint, new Point(cx, cy));
        if (gsp == gap.EndPoint) {
            var mpx = cx + (cx - gsp.X);
            var mpy = cy + (cy - gsp.Y);
            var part1 = new Arc {
                RotationDirection = arcWay.RotationDirection,
                Radius = tr,
                IsLargeArc = false,
                PointFrom = gsp,
                PointTo = new Point(mpx, mpy),
            };
            var part2 = new Arc {
                RotationDirection = arcWay.RotationDirection,
                Radius = tr,
                IsLargeArc = true,
                PointFrom = new Point(mpx, mpy),
                PointTo = gsp,
            };
            result.Add(part1);
            result.Add(part2);
        } else {
            
            var part = new Arc {
                RotationDirection = arcWay.RotationDirection, //Invert, because gerber and svg has different axis layout
                PointTo = gap.EndPoint,
                Radius = tr,
                IsLargeArc = arcWay.IsLarge,
                PointFrom = gsp,
            };
            result.Add(part);
        }
        return result;
    }


}

