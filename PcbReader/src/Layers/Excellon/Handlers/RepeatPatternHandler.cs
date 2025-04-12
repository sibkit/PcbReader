using PcbReader.Layers.Excellon.Entities;
using ApplicationException = System.ApplicationException;

namespace PcbReader.Layers.Excellon.Handlers;

public partial class RepeatPatternHandler: ICommandHandler<ExcellonCommandType, ExcellonContext, ExcellonLayer> {
    
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.RepeatPattern];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine.StartsWith("M02");
    }
    
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        if (ctx.CurLine == "M02") {
            if (ctx.CurPattern == null || ctx.CurPattern.State == PatternState.Opened)
                throw new ApplicationException("Pattern is null or opened (1)");
            ctx.CurPattern = null;
        } else {
            var sc = ctx.CurLine.Split("M02", StringSplitOptions.RemoveEmptyEntries)[0];
            var readedPoint = ExcellonCoordinates.ReadCoordinate(sc, ctx);
            if (readedPoint == null) throw new ApplicationException("Readed point is null");
            if (ctx.CurPattern == null || ctx.CurPattern.State == PatternState.Opened)
                throw new ApplicationException("Pattern is null or opened (2)");

            var pattern = ctx.CurPattern!;
            foreach (var operation in pattern.MachiningOperations) {
                layer.Operations.Add(operation.CloneWithShift(readedPoint.Value));
            }
        }
    }
}