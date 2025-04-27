using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using PcbReader.Geometry;
using PcbReader.Layers.Common;
using PcbReader.Layers.Svg.Entities;

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

    private static Rect CalculateViewBox(SvgLayer doc) {

        var leftTop = new Point(double.MaxValue, double.MaxValue);
        var rightBottom = new Point(double.MinValue, double.MinValue);
        

        foreach (var p in doc.Paths) {
            
            foreach (var pp in p.Parts) {
                if (pp is ISvgCursorDriver cursorDriver)
                    ExtendBounds(ref leftTop, ref rightBottom, cursorDriver.PointTo);
            }
        }

        var result = new Rect {
            StartPoint = leftTop,
            EndPoint = rightBottom,
        };
        var maxSide = result.GetWidth() > result.GetHeight() ? result.GetWidth() * 0.08 : result.GetHeight() * 0.08;
        result.StartPoint = result.StartPoint with {
            X = result.StartPoint.X - maxSide,
            Y = result.StartPoint.Y - maxSide
        };
        result.EndPoint = result.EndPoint with {
            X = result.EndPoint.X + maxSide,
            Y = result.EndPoint.Y + maxSide
        };
        return result;
    }

    public static void Write(SvgLayer doc, string fileName) {
        using var swr = new StreamWriter(fileName);
        var vbr = doc.ViewBox ?? CalculateViewBox(doc);

        swr.Write("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\""+
                  vbr.StartPoint.X.ToString("R")+ " "+
                  vbr.StartPoint.Y.ToString("R")+" "+ 
                  Math.Round(vbr.GetWidth(),6) + " "+
                  Math.Round(vbr.GetHeight(),6) + "\">");
        swr.Write("\n<g fill=\"none\" stroke=\"red\"  stroke-linejoin=\"round\" stroke-linecap=\"round\" stroke-width=\""+Math.Round(vbr.GetWidth()/1000,6)+"\">");
        foreach (var path in doc.Paths) {
            //var sp = path.StartPoint;
            swr.Write("\n<path d=\"");
            foreach (var pp in path.Parts) {
                switch (pp) {
                    case MoveSvgPathPart m:
                        swr.Write("M  " + Math.Round(m.PointTo.X, 6) + " " + Math.Round(m.PointTo.Y, 6) + " ");
                        break;
                    case LineSvgPathPart l:
                        swr.Write("L " + Math.Round(l.PointTo.X, 6) + " " + Math.Round(l.PointTo.Y, 6) + " ");
                        break;
                    case ArcSvgPathPart a:
                        swr.Write("A " +
                                  Math.Round(a.Radius, 8) + " " +
                                  Math.Round(a.Radius, 8) + " " +
                                  "0 " +
                                  (a.IsLargeArc ? "1 " : "0 ") +
                                  (a.RotationDirection == RotationDirection.ClockWise ? "1 " : "0 ") +
                                  Math.Round(a.PointTo.X, 8) + " " +
                                  Math.Round(a.PointTo.Y, 8) + " ");
                        break;
                    case CloseSvgPathPart:
                        swr.Write("Z");
                        break;
                }
            }
            swr.Write("\"");
            if (path.Parts.Count >0 && path.Parts.Last() is CloseSvgPathPart) {
                swr.Write(" fill=\"red\"");
            }
            if (path.StrokeWidth > 0.00000001) {
                swr.Write( "stroke-width=\""+Math.Round(path.StrokeWidth,8)+"\"");
            }
            swr.Write("/>");
        }
        swr.Write("\n</g>");
        swr.Write("\n</svg>");
        swr.Flush();
    }
}