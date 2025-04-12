using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Handlers;

public class SetLcModeHandler: ICommandHandler<GerberCommandType, GerberContext, GerberLayer> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return[];
    }
    public bool Match(GerberContext ctx) {
        return ctx.CurLine is "G01" or "G02" or "G03";
    }
    public void WriteToProgram(GerberContext ctx, GerberLayer program) {
        ctx.LcMode = ctx.CurLine switch {
            "G01" => LcMode.Linear,
            "G02" => LcMode.Clockwise,
            "G03" => LcMode.Counterclockwise,
            _ => throw new Exception("Unknown LC mode")
        };
    }
}