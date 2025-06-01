
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using PcbReader.Core;
using PcbReader.Core.GraphicElements;
using PcbReader.Core.GraphicElements.PathParts;
using PcbReader.Layers.Svg.Entities;
using Path = PcbReader.Core.GraphicElements.Path;

namespace PcbReader.Layers.Svg.Writing;

[SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
public static class SvgWriter {

    static SvgWriter() {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
    }

    private static Bounds ExtendBounds(Bounds bounds, Point pt) {
        return new Bounds(
            bounds.MinX < pt.X ? bounds.MinX : pt.X,
            bounds.MinY < pt.Y ? bounds.MinY : pt.Y,
            bounds.MaxX > pt.X ? bounds.MaxX : pt.X,
            bounds.MaxY > pt.Y ? bounds.MaxY : pt.Y
        );
    }

    private static Bounds CalculateViewBox(SvgLayer doc) {
        // var leftTop = new Point(double.MaxValue, double.MaxValue);
        // var rightBottom = new Point(double.MinValue, double.MinValue);

        var result = new Bounds(double.MaxValue, double.MaxValue, double.MinValue, double.MinValue);
        
        foreach (var e in doc.Elements) {
            switch (e) {
                case Path p:
                    foreach (var pp in p.Parts) {
                        result = ExtendBounds(result, pp.PointFrom);
                        result = ExtendBounds(result, pp.PointTo);
                    }

                    break;
                case Shape shape:

                    result = ExtendBounds(result, shape.OuterContour.Parts[0].PointFrom);
                    foreach (var pp in shape.OuterContour.Parts) {
                        result = ExtendBounds(result, pp.PointFrom);
                        result = ExtendBounds(result, pp.PointTo);
                    }
                        

                    foreach (var ic in shape.InnerContours) {

                        foreach (var pp in ic.Parts) {
                            result = ExtendBounds(result, ic.Parts[0].PointFrom);
                            result = ExtendBounds(result, pp.PointTo);
                        }
                            
                    }

                    break;
                case Contour contour:

                    foreach (var pp in contour.Parts) {
                        result = ExtendBounds(result, pp.PointTo);
                        result = ExtendBounds(result, contour.Parts[0].PointFrom);
                    }
                        
                    break;
                case Dot dot:
                    result = ExtendBounds(result, dot.CenterPoint);
                    break;
                default: throw new NotImplementedException();
            }
        }


        var field = result.GetWidth() > result.GetHeight() ? result.GetWidth() * 0.04 : result.GetHeight() * 0.04;
        result = new Bounds(
            result.MinPoint.X - field,
            result.MinPoint.Y - field,
            result.MaxPoint.X + field,
            result.MaxPoint.Y + field
        );

        return result;
    }

    private static List<Bounds> _pathPartsBounds = [];
    static void WritePathPart(StreamWriter writer, ICurve curve) {
        switch (curve) {
            case Line l:
                writer.Write("L " + Math.Round(l.PointTo.X, 6) + " " + Math.Round(l.PointTo.Y, 6) + " ");
                break;
            case Arc a:
                writer.Write("A " +
                             Math.Round(a.Radius, 8) + " " +
                             Math.Round(a.Radius, 8) + " " +
                             "0 " +
                             (a.IsLargeArc ? "1 " : "0 ") +
                             (a.RotationDirection == RotationDirection.Clockwise ? "0 " : "1 ") +
                             Math.Round(a.PointTo.X, 8) + " " +
                             Math.Round(a.PointTo.Y, 8) + " ");
                break;
        }
        _pathPartsBounds.Add(curve.Bounds);
    }

    static void WritePath(StreamWriter writer, Path path) {
        //var sp = path.StartPoint;
        if(path.Parts.Count==0)
            return;
        var firstPart = path.Parts[0];
        
        writer.Write("\n<path d=\"M  " + Math.Round(firstPart.PointFrom.X, 6) + " " + Math.Round(firstPart.PointFrom.Y, 6) + " ");
        foreach (var pp in path.Parts) 
            WritePathPart(writer, pp);
        if (path.StrokeWidth > 0.00000001) {
            writer.Write("\" stroke-width=\""+Math.Round(path.StrokeWidth,8)+"\"");
        }
        writer.Write("/>");
    }

    static void WriteContour(StreamWriter writer, Contour contour) {
        if(contour.Parts.Count==0)
            return;
        var sp = contour.Parts[0].PointFrom;
        writer.Write("\n<path d=\"M  " + Math.Round(sp.X, 6) + " " + Math.Round(sp.Y, 6) + " ");
        foreach (var pp in contour.Parts) 
            WritePathPart(writer, pp);
        writer.Write("Z\" fill=\"black\"/>");
    }

    static void WriteShape(StreamWriter writer, Shape shape) {
            WriteContour(writer, shape.OuterContour);
    }

    static void WriteDot(StreamWriter writer, Dot dot) {
        writer.Write("<circle cx=\"" + Math.Round(dot.CenterPoint.X, 6) + "\" cy=\"" + Math.Round(dot.CenterPoint.Y, 6) + "\" r=\"" + Math.Round(dot.Diameter / 2, 6) + "\" fill=\"red\"/>");
    }

    public static void Write(SvgLayer doc, string fileName) {
        using var swr = new StreamWriter(fileName);
        
        var vbr = doc.ViewBox ?? CalculateViewBox(doc);

        swr.Write("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\""+
                   Math.Round(vbr.MinPoint.X,6)+ " "+
                  Math.Round(vbr.MinPoint.Y,6)+" "+ 
                  Math.Round(vbr.GetWidth(),6) + " "+
                  Math.Round(vbr.GetHeight(),6) + "\">");
        swr.Write("\n<g fill=\"none\" stroke=\"red\" stroke-linejoin=\"round\" stroke-linecap=\"round\" stroke-width=\"0\">");
        foreach (var e in doc.Elements) {
            switch (e) {
                case Path p:
                    WritePath(swr, p);
                    break;
                case Contour c:
                    WriteContour(swr, c);
                    break;
                case Shape sh:
                    WriteShape(swr, sh);
                    break;
                case Dot dot:
                    WriteDot(swr, dot);
                    break;
            }
            AddBoundsRect(swr,e.GetBounds(),"yellow");
        }

        foreach (var b in _pathPartsBounds) {
            AddBoundsRect(swr,b,"orange");
        }
        
        swr.Write("\n</g>");
        swr.Write("\n</svg>");
        swr.Flush();
    }

    static void AddBoundsRect(StreamWriter writer, Bounds b, string color) {
        writer.Write("\n<rect x=\""+Math.Round(b.MinX)+"\" y=\""+Math.Round(b.MinY)+"\" width=\""+Math.Round(b.GetWidth())+"\" height=\""+Math.Round(b.GetHeight())+"\" fill=\"none\" stroke=\""+color+"\" stroke-width=\"0.4px\"/>");
    }
}