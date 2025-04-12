using PcbReader.Layers.Excellon.Entities;

namespace PcbReader.Layers.Excellon.Handlers;

public class SetDrillModeHandler: ICommandHandler<ExcellonCommandType, ExcellonContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.DrillOperation];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine == "G05";
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        ctx.CurOperationType = MachiningOperationType.Drill;
    }
}