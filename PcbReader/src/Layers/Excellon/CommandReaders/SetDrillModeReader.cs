using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Excellon.Entities;

namespace PcbReader.Layers.Excellon.Handlers;

public class SetDrillModeReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.DrillOperation];
    }
    public bool Match(ExcellonReadingContext ctx) {
        return ctx.CurLine == "G05";
    }
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        ctx.CurOperationType = MachiningOperationType.Drill;
    }
}