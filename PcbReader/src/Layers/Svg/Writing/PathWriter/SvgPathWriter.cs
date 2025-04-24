using PcbReader.Layers.Svg.Entities;
using Path = PcbReader.Layers.Svg.Entities.Path;

namespace PcbReader.Layers.Svg.Writing.PathWriter;

public static class SvgPathWriter {
    public static void WritePath(Path p, TextWriter writer) {
        writer.Write("<path ");
        
        writer.WriteLine("M "+p.StartPoint.X + p.StartPoint.Y);
    }

    static void WriteArcPatchPart(TextWriter w, ArcPathPart part) {
        w.Write("A");
        
        w.Write(part.EndPoint.X);
        w.Write(" "+part.EndPoint.Y+" ");
    }

    static void WriteLinePathPart(TextWriter w, LinePathPart part) {
        w.Write("L");
        w.Write(part.EndPoint.X);
        w.Write(" "+part.EndPoint.Y+" ");
    }
}