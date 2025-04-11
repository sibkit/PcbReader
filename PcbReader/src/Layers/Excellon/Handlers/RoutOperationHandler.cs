namespace PcbReader.Layers.Excellon.Handlers;

public class RoutOperationHandler: ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer> {
    public ExcellonLineType[] GetNextLikelyTypes() {
        return [ExcellonLineType.StartMill];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine.StartsWith("G00");
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        if (ctx.CurLine == "G00") {
            return;
        }
        var sc = ctx.CurLine[3..];
        var coordinate = ExcellonCoordinates.ReadCoordinate(sc, ctx);
        if (coordinate == null) {
            ctx.WriteError("Invalid coordinate: "+ctx.CurLine);
        } else {
            ctx.CurCoordinate = coordinate.Value;
        }
        
    }
}