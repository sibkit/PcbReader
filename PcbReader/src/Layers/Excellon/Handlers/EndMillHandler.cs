﻿namespace PcbReader.Layers.Excellon.Handlers;

public class EndMillHandler: ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer> {
    public ExcellonLineType[] GetNextLikelyTypes() {
        return [ExcellonLineType.RoutOperation, ExcellonLineType.DrillOperation, ExcellonLineType.SetTool];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine is "M16" or "M17";
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
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