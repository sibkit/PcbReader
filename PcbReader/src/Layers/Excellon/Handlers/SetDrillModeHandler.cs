using PcbReader.Layers.Excellon.Entities;

namespace PcbReader.Layers.Excellon.Handlers;

public class SetDrillModeHandler: ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer> {
    public ExcellonLineType[] GetNextLikelyTypes() {
        return [ExcellonLineType.DrillOperation];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine == "G05";
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        ctx.CurOperationType = MachiningOperationType.Drill;
    }
}