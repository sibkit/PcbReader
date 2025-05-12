
using PcbReader.Geometry;
using PcbReader.Geometry.PathParts;
using PcbReader.Layers.Svg.Entities;
using Path = PcbReader.Geometry.Path;

namespace PcbReader.Layers.Svg.Writing;

public static class SvgWriter {

    static SvgWriter() {
        
    }
    
    private static void ExtendBounds(ref Point leftTop, ref Point rightBottom, Point pt) {
        if(pt.X < leftTop.X)
            leftTop.X = pt.X;
        if(pt.X > rightBottom.X)
            rightBottom.X = pt.X;
        
        if(pt.Y < leftTop.Y)
            leftTop.Y = pt.Y;
        if(pt.Y > rightBottom.Y)
            rightBottom.Y = pt.Y;
    }

    private static Bounds CalculateViewBox(SvgLayer doc) {
        var leftTop = new Point(double.MaxValue, double.MaxValue);
        var rightBottom = new Point(double.MinValue, double.MinValue);

        foreach (var e in doc.Elements) {
            switch (e) {
                case Path p:
                    foreach (var pp in p.Segments) {
                        ExtendBounds(ref leftTop, ref rightBottom, pp.PointTo);
                    }

                    break;
                case Shape shape:

                    ExtendBounds(ref leftTop, ref rightBottom, shape.OuterContour.StartPoint);
                    foreach (var pp in shape.OuterContour.Parts)
                        ExtendBounds(ref leftTop, ref rightBottom, pp.PointTo);

                    foreach (var ic in shape.InnerContours) {
                        ExtendBounds(ref leftTop, ref rightBottom, ic.StartPoint);
                        foreach (var pp in ic.Parts)
                            ExtendBounds(ref leftTop, ref rightBottom, pp.PointTo);
                    }

                    break;
                case Contour contour:
                    ExtendBounds(ref leftTop, ref rightBottom, contour.StartPoint);
                    foreach (var pp in contour.Parts)
                        ExtendBounds(ref leftTop, ref rightBottom, pp.PointTo);
                    break;
                case Dot dot:
                    ExtendBounds(ref leftTop, ref rightBottom, dot.Point);
                    break;
                default: throw new NotImplementedException();
            }
        }

        var result = new Bounds {
            StartPoint = leftTop,
            EndPoint = rightBottom,
        };
        var field = result.GetWidth() > result.GetHeight() ? result.GetWidth() * 0.04 : result.GetHeight() * 0.04;
        result.StartPoint = result.StartPoint with {
            X = result.StartPoint.X - field,
            Y = result.StartPoint.Y - field
        };
        result.EndPoint = result.EndPoint with {
            X = result.EndPoint.X + field,
            Y = result.EndPoint.Y + field
        };
        return result;
    }

    static void WritePathPart(StreamWriter writer, IPathPart pathPart) {
        switch (pathPart) {
            case LinePathPart l:
                writer.Write("L " + Math.Round(l.PointTo.X, 6) + " " + Math.Round(l.PointTo.Y, 6) + " ");
                break;
            case ArcPathPart a:
                writer.Write("A " +
                             Math.Round(a.Radius, 8) + " " +
                             Math.Round(a.Radius, 8) + " " +
                             "0 " +
                             (a.IsLargeArc ? "1 " : "0 ") +
                             (a.RotationDirection == RotationDirection.ClockWise ? "1 " : "0 ") +
                             Math.Round(a.PointTo.X, 8) + " " +
                             Math.Round(a.PointTo.Y, 8) + " ");
                break;
        }
    }

    static void WritePath(StreamWriter writer, Path path) {
        var sp = path.StartPoint;
        writer.Write("\n<path d=\"M  " + Math.Round(sp.X, 6) + " " + Math.Round(sp.Y, 6) + " ");
        foreach (var pp in path.Segments) 
            WritePathPart(writer, pp);
        if (path.StrokeWidth > 0.00000001) {
            writer.Write("\" stroke-width=\""+Math.Round(path.StrokeWidth,8)+"\"");
        }
        writer.Write("/>");
    }

    static void WriteContour(StreamWriter writer, Contour contour) {
        var sp = contour.StartPoint;
        writer.Write("\n<path d=\"M  " + Math.Round(sp.X, 6) + " " + Math.Round(sp.Y, 6) + " ");
        foreach (var pp in contour.Parts) 
            WritePathPart(writer, pp);
        writer.Write("Z\" fill=\"black\"/>");
    }

    static void WriteShape(StreamWriter writer, Shape shape) {
            WriteContour(writer, shape.OuterContour);
    }

    static void WriteDot(StreamWriter writer, Dot dot) {
        writer.Write("<circle cx=\"" + Math.Round(dot.Point.X, 6) + "\" cy=\"" + Math.Round(dot.Point.Y, 6) + "\" r=\"" + Math.Round(dot.Diameter / 2, 6) + "\" fill=\"black\"/>");
    }

    public static void Write(SvgLayer doc, string fileName) {
        using var swr = new StreamWriter(fileName);
        var vbr = doc.ViewBox ?? CalculateViewBox(doc);

        swr.Write("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\""+
                  Math.Round(vbr.StartPoint.X,6)+ " "+
                  Math.Round(vbr.StartPoint.Y,6)+" "+ 
                  Math.Round(vbr.GetWidth(),6) + " "+
                  Math.Round(vbr.GetHeight(),6) + "\">");
        swr.Write("\n<g fill=\"none\" stroke=\"red\"  stroke-linejoin=\"round\" stroke-linecap=\"round\" stroke-width=\"0\">");
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
        }
        swr.Write("\n</g>");
        swr.Write("\n</svg>");
        swr.Flush();
    }
}