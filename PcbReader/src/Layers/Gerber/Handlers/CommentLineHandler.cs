using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Handlers;

public class CommentLineHandler: ILineHandler<GerberLineType, GerberContext, GerberLayer> {
    public GerberLineType[] GetNextLikelyTypes() {
        return [GerberLineType.Comment];
    }
    public bool Match(GerberContext ctx) {
        return ctx.CurLine.StartsWith("G04");
    }
    public void WriteToProgram(GerberContext ctx, GerberLayer program) {
        ctx.WriteInfo(ctx.CurLine[3..]);
    }
}