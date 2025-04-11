namespace PcbReader.Layers.Excellon.Handlers;

public class EndProgramHandler: ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer> {
    public ExcellonLineType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine == "M30";
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        //Do nothing
    }
}