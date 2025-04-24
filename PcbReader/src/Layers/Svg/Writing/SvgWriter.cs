using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using PcbReader.Layers.Common;
using PcbReader.Layers.Svg.Entities;

namespace PcbReader.Layers.Svg.Writing;

public static class SvgWriter {

    static SvgWriter() {
        
    }
    
    private static void ExtendBounds(ref Rect bounds, Point pt) {
        if(pt.X < bounds.StartPoint.X)
            bounds.StartPoint = bounds.StartPoint with { X = pt.X };
        if(pt.X > bounds.EndPoint.X)
            bounds.EndPoint = bounds.EndPoint with { X = pt.X };
        
        if(pt.Y < bounds.StartPoint.Y)
            bounds.StartPoint = bounds.StartPoint with { Y = pt.Y };
        if(pt.Y > bounds.EndPoint.Y)
            bounds.EndPoint = bounds.EndPoint with { Y = pt.Y };
    }
    
    private static Rect CalculateViewBox(SvgLayer doc) {
        var result = new Rect();
        foreach (var p in doc.Paths) {
            ExtendBounds(ref result, p.StartPoint);
            foreach (var pp in p.Parts) {
                ExtendBounds(ref result, pp.EndPoint);
            }
        }
        var maxSide =  result.GetWidth() > result.GetHeight() ? result.GetWidth()*0.08 : result.GetHeight()*0.08;
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
                  Math.Round(vbr.GetWidth(),8) + " "+
                  Math.Round(vbr.GetHeight(),8) + "\">");
        foreach (var path in doc.Paths) {
            var sp = path.StartPoint;
            swr.Write("\n<path d=\"M ");
            swr.Write(sp.X);
            swr.Write(" "+sp.Y+" ");
            foreach (var pp in path.Parts) {
                switch (pp) {
                    case LinePathPart l:
                        swr.Write("L " + pp.EndPoint.X + " " + pp.EndPoint.Y + " ");
                        break;
                    case ArcPathPart a:
                        swr.Write("A " +
                                  Math.Round(a.Radius, 8) + " " +
                                  Math.Round(a.Radius, 8) + " " +
                                  (a.RotationDirection == RotationDirection.ClockWise ? "0" : "1") +
                                  " 0 0 " +
                                  Math.Round(a.EndPoint.X, 8) + " " +
                                  Math.Round(a.EndPoint.Y, 8) + " ");
                        break;
                }
            }

            swr.Write("Z\"/>");
        }
        swr.Write("\n</svg>");
        swr.Flush();
    }
}