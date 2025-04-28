using PcbReader.Layers.Common.Reading;

namespace PcbReader.Layers.Excellon.Handlers;

public class EndMillReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.RoutOperation, ExcellonCommandType.DrillOperation, ExcellonCommandType.SetTool];
    }
    public bool Match(ExcellonReadingContext ctx) {
        return ctx.CurLine is "M16" or "M17";
    }
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        if (ctx.CurLine == "M17" && ctx.Lines[ctx.CurIndex-1] == "M16") {
            return;
        }
        if (ctx.CurMillOperation == null) {
            ctx.WriteError("Завершение не начатой операции фрезерования.");
            ctx.CurMillOperation = null;
            return;
        }
        layer.Operations.Add(ctx.CurMillOperation);
        ctx.CurMillOperation = null;
    }
}