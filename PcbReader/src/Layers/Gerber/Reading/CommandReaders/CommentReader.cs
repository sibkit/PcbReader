using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public class CommentReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return [GerberCommandType.Comment];
    }
    public bool Match(GerberReadingContext ctx) {
        return ctx.CurLine.StartsWith("G04");
    }
    public void WriteToProgram(GerberReadingContext ctx, GerberLayer program) {
        ctx.WriteInfo(ctx.CurLine[3..^1]);
    }
}