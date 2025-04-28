using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Excellon.Entities;

namespace PcbReader.Layers.Excellon.Handlers;

public class StartMillReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.LinearMillOperation, ExcellonCommandType.ArcMillOperation, ExcellonCommandType.EndMill];
    }
    public bool Match(ExcellonReadingContext ctx) {
        return ctx.CurLine == "M15";
    }
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        if (ctx.CurMillOperation != null) {
            ctx.WriteError("Начало фрезерования при незавершенном фрезеровании.");
        }
        ctx.CurMillOperation = new MillOperation() {
            StartPoint = ctx.CurPoint
        };
    }
}