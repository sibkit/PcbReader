
using PcbReader.Geometry;
using PcbReader.Layers.Common;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Svg;
using PcbReader.Layers.Svg.Entities;
using GerberArcPart = PcbReader.Layers.Gerber.Entities.ArcPathPart;
using GerberLinePart = PcbReader.Layers.Gerber.Entities.LinePathPart;

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
                    var aperture = layer.Apertures[flash.ApertureCode];
                    result.Paths.Add(ApertureConverter.ConvertAperture(flash.Point, aperture));
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: Convert");
            }
        }
        InvertAxis(result);
        return result;
    }

    // static Point SvgPoint(Point gerberPoint) {
    //     return new Point {
    //         X = gerberPoint.X,
    //         Y = -1d * gerberPoint.Y
    //     };
    // }

    static SvgPath ConvertPath(PathPaintOperation operation) {
        var result = new SvgPath {
            StartPoint = operation.StartPoint, 
        };
        var startPartPoint = operation.StartPoint;
        foreach (var op in operation.Parts) {
            switch (op) {
                case GerberLinePart line:
                    result.Parts.Add(new LineSvgPathPart {
                        PointTo = line.EndPoint
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
        if(operation.IsClosed)
            result.Parts.Add(new CloseSvgPathPart());
        return result;
    }
    
    static List<ArcSvgPathPart> ConvertArcPath(Point gsp, GerberArcPart gap) {
        var result = new List<ArcSvgPathPart>();
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
        var tr = (decimal)(r2 + r1) / 2; //true radius
        var arcWay = Geometry.Geometry.ArcWay(gsp, gap.EndPoint, new Point(cx, cy), AxisLayout.YDownXRight);
        if (gsp == gap.EndPoint) {
            var mpx = cx + (cx - gsp.X);
            var mpy = cy + (cy - gsp.Y);
            var part1 = new ArcSvgPathPart {
                RotationDirection = arcWay.RotationDirection,
                Radius = tr,
                IsLargeArc = false,
                PointTo = new Point(mpx, mpy)
            };
            var part2 = new ArcSvgPathPart {
                RotationDirection = arcWay.RotationDirection,
                Radius = tr,
                IsLargeArc = true,
                PointTo = gsp
            };
            result.Add(part1);
            result.Add(part2);
        } else {
            
            var part = new ArcSvgPathPart {
                RotationDirection = arcWay.RotationDirection, //Invert, because gerber and svg has different axis layout
                PointTo = gap.EndPoint,
                Radius = tr,
                IsLargeArc = arcWay.IsLarge,
            };
            result.Add(part);
        }
        return result;
    }

    static void InvertAxis(SvgLayer layer) {
        foreach (var pth in layer.Paths) {
            pth.StartPoint = pth.StartPoint with { Y = -pth.StartPoint.Y };
            foreach (var p in pth.Parts) {
                switch (p) {
                    case LineSvgPathPart line:
                        line.PointTo = line.PointTo with { Y = -line.PointTo.Y };
                        break;
                    case ArcSvgPathPart arc:
                        arc.PointTo = arc.PointTo with { Y = -arc.PointTo.Y };
                        arc.RotationDirection = arc.RotationDirection.Invert();
                        break;
                }
            }
        }
    }
}

