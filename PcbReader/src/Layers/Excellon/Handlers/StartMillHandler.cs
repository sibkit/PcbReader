using PcbReader.Layers.Excellon.Entities;

namespace PcbReader.Layers.Excellon.Handlers;

public class StartMillHandler: ICommandHandler<ExcellonCommandType, ExcellonContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.LinearMillOperation, ExcellonCommandType.ArcMillOperation, ExcellonCommandType.EndMill];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine == "M15";
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        if (ctx.CurMillOperation != null) {
            ctx.WriteError("Начало фрезерования при незавершенном фрезеровании.");
        }
        ctx.CurMillOperation = new MillOperation() {
            StartCoordinate = ctx.CurCoordinate
        };
    }
}