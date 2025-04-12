using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Handlers;

public class CommentCommandHandler: ICommandHandler<GerberCommandType, GerberContext, GerberLayer> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return [GerberCommandType.Comment];
    }
    public bool Match(GerberContext ctx) {
        return ctx.CurLine.StartsWith("G04");
    }
    public void WriteToProgram(GerberContext ctx, GerberLayer program) {
        ctx.WriteInfo(ctx.CurLine[3..^1]);
    }
}