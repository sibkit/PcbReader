using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Handlers;

public class SetCoordinateModeHandler: ICommandHandler<GerberCommandType, GerberContext, GerberLayer> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberContext ctx) {
        return ctx.CurLine is "G90*" or "G91*";
    }
    public void WriteToProgram(GerberContext ctx, GerberLayer program) {
        ctx.CoordinatesMode = ctx.CurLine switch {
            "G90*" => CoordinatesMode.Absolute,
            "G91*" => CoordinatesMode.Incremental,
            _ => throw new Exception("Unknown coordinates mode")
        };
    }
}