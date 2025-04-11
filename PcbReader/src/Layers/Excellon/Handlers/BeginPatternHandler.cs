using PcbReader.Layers.Excellon.Entities;
using PcbReader.Project;
using ApplicationException = System.ApplicationException;

namespace PcbReader.Layers.Excellon.Handlers;

public class BeginPatternHandler: ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer> {
    public ExcellonLineType[] GetNextLikelyTypes() {
        return [ExcellonLineType.DrillOperation];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine == "M25";
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        if (ctx.CurPattern == null || ctx.CurPattern.State == PatternState.Closed) {
            var coordinate = new Coordinate(0,0);
            if (layer.Operations.Count != 0) {
                var lo = layer.Operations.Last();
                coordinate = lo.StartCoordinate;
            }
            
            ctx.CurPattern = new Pattern(coordinate);
            
        } else {
            throw new ApplicationException("Команда открытия шаблона при уже открытом шаблоне.");
        }
    }
}