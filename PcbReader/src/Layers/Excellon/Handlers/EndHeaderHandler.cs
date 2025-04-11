namespace PcbReader.Layers.Excellon.Handlers;

public class EndHeaderHandler: ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer> {
    public ExcellonLineType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(ExcellonContext ctx) {
        var line = ctx.CurLine;
        return line.Equals("%") || line.Equals("M95");
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        //do nothing
    }
}