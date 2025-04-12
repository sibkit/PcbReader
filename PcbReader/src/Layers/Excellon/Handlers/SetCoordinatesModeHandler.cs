namespace PcbReader.Layers.Excellon.Handlers;

public class SetCoordinatesModeHandler: ICommandHandler<ExcellonCommandType, ExcellonContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.DrillOperation];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine is "G90" or "G91" or "ICI,OFF" or "ICI,ON" or "ICI";
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        ctx.CoordinatesMode = ctx.CurLine switch {
            "G90" => CoordinatesMode.Absolute,
            "G91" => CoordinatesMode.Incremental,
            "ICI" => CoordinatesMode.Incremental,
            "ICI,ON" => CoordinatesMode.Incremental,
            "ICI,OFF" => CoordinatesMode.Absolute,
            _ => throw new Exception("Unknown CoordinatesMode: " + ctx.CurLine)
        };
    }
}