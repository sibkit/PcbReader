namespace PcbReader.Layers.Excellon.Handlers;

public class EndProgramHandler: ICommandHandler<ExcellonCommandType, ExcellonContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine == "M30";
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        //Do nothing
    }
}