using PcbReader.Layers.Excellon.Entities;

namespace PcbReader.Layers.Excellon.Handlers;

public class EndPatternHandler: ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer> {
    public ExcellonLineType[] GetNextLikelyTypes() {
        return [ExcellonLineType.RepeatPattern];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine is "M01" or "M08";
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        switch (ctx.CurLine) {
            case "M01": {
                if (ctx.CurPattern == null || ctx.CurPattern.State == PatternState.Closed) {
                    ctx.WriteError("Команда закрытия шаблона, без его открытия");
                    return;
                }
                
                ctx.CurPattern.State = PatternState.Closed;
                break;
            }
            case "M08":
                ctx.CurPattern = null;
                break;
        }
    }
}