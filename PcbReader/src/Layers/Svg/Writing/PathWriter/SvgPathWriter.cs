using PcbReader.Layers.Svg.Entities;

namespace PcbReader.Layers.Svg.Writing.PathWriter;

public static class SvgPathWriter {
    public static void WritePath(SvgPath p, TextWriter writer) {
        writer.Write("<path ");
        
        writer.WriteLine("M "+p.StartPoint.X + p.StartPoint.Y);
    }

    static void WriteArcPatchPart(TextWriter w, ArcSvgPathPart part) {
        w.Write("A");
        
        w.Write(part.PointTo.X);
        w.Write(" "+part.PointTo.Y+" ");
    }

    static void WriteLinePathPart(TextWriter w, LineSvgPathPart part) {
        w.Write("L");
        w.Write(part.PointTo.X);
        w.Write(" "+part.PointTo.Y+" ");
    }
}