
using System.Net.Http.Headers;
using PcbReader.Geometry;
using PcbReader.Geometry.PathParts;
using PcbReader.Layers.Common;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Svg;
using PcbReader.Layers.Svg.Entities;
using ArcPathPart = PcbReader.Geometry.PathParts.ArcPathPart;
using GerberArcPart = PcbReader.Layers.Gerber.Entities.ArcPathPart;
using GerberLinePart = PcbReader.Layers.Gerber.Entities.LinePathPart;
using IPathPart = PcbReader.Geometry.IPathPart;
using LinePathPart = PcbReader.Geometry.PathParts.LinePathPart;
using Path = PcbReader.Geometry.Path;

namespace PcbReader.Converters.GerberToSvg;

public static class GerberToSvgConverter {
    
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
        InvertAxis(result);
        return result;
    }
    
    static Path ConvertPath(PathPaintOperation operation) {
        var result = new Path {
            StrokeWidth = operation.Aperture.Diameter,
            StartPoint = operation.StartPoint,
        };

        var startPartPoint = operation.StartPoint;
        foreach (var op in operation.Parts) {
            switch (op) {
                case GerberLinePart line:
                    result.Parts.Add(new LinePathPart {
                        PointFrom = startPartPoint,
                        PointTo = line.EndPoint,
                    });
                    break;
                case GerberArcPart arc:
                    result.Parts.AddRange(ConvertArcPath(startPartPoint, arc, result));
                    startPartPoint = arc.EndPoint;
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: ConvertPath");
            }
        }
        return result;
    }
    
    static List<ArcPathPart> ConvertArcPath(Point gsp, GerberArcPart gap, IPathPartsOwner owner) {
        var result = new List<ArcPathPart>();
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
        var arcWay = Geometry.Geometry.ArcWay(gsp, gap.EndPoint, new Point(cx, cy));
        if (gsp == gap.EndPoint) {
            var mpx = cx + (cx - gsp.X);
            var mpy = cy + (cy - gsp.Y);
            var part1 = new ArcPathPart {
                RotationDirection = arcWay.RotationDirection,
                Radius = tr,
                IsLargeArc = false,
                PointFrom = gsp,
                PointTo = new Point(mpx, mpy),
                Owner = owner
            };
            var part2 = new ArcPathPart {
                RotationDirection = arcWay.RotationDirection,
                Radius = tr,
                IsLargeArc = true,
                PointFrom = new Point(mpx, mpy),
                PointTo = gsp,
                Owner = owner
            };
            result.Add(part1);
            result.Add(part2);
        } else {
            
            var part = new ArcPathPart {
                RotationDirection = arcWay.RotationDirection, //Invert, because gerber and svg has different axis layout
                PointTo = gap.EndPoint,
                Radius = tr,
                IsLargeArc = arcWay.IsLarge,
                PointFrom = gsp,
                Owner = owner,
            };
            result.Add(part);
        }
        return result;
    }


    static IPathPart InvertAxis(IPathPart pathPart, IPathPartsOwner newOwner) {
        return pathPart switch {
            LinePathPart line => new LinePathPart {
                PointTo = line.PointTo.WithNewY(-line.PointTo.Y), 
                PointFrom = line.PointFrom.WithNewY(-line.PointFrom.Y)
            },
            ArcPathPart arc => new ArcPathPart {
                PointTo = arc.PointTo.WithNewY(-arc.PointTo.Y),
                PointFrom = arc.PointFrom.WithNewY(-arc.PointFrom.Y),
                IsLargeArc = arc.IsLargeArc,
                RotationDirection = arc.RotationDirection.Invert(),
                Owner = newOwner,
                Radius = arc.Radius,
            },
            _ => throw new Exception("GerberToSvgConverter: InvertAxis")
        };
    }

    static void InvertAxis(IPathPartsOwner ctx) {
        ctx.StartPoint = ctx.StartPoint.WithNewY(-ctx.StartPoint.Y);
        foreach (var p in ctx.Parts) {
            InvertAxis(p, ctx);
        }
    }

    static void InvertAxis(SvgLayer layer) {
        foreach (var e in layer.Elements) {
            switch (e) {

                case IPathPartsOwner ctr:
                    InvertAxis(ctr);
                    break;
                case Shape shape:
                        InvertAxis(shape.OuterContour);
                    foreach (var ic in shape.InnerContours)
                        InvertAxis(ic);
                    break;
                default:
                    throw new Exception("GerberToSvgConverter: InvertAxis");
            }
        }
    }
}

