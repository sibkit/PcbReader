using PcbReader.Layers.Excellon.Entities;

namespace PcbReader.Layers.Excellon.Handlers;

public class LinearMillOperationHandler: ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer> {
    public ExcellonLineType[] GetNextLikelyTypes() {
        return [ExcellonLineType.LinearMillOperation, ExcellonLineType.ArcMillOperation];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine.StartsWith("G01");
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        var sc = ctx.CurLine[3..];
        var coordinate = ExcellonCoordinates.ReadCoordinate(sc, ctx);
        if (coordinate == null) {
            ctx.WriteError("Invalid coordinate: "+ctx.CurLine);
        } else {
            if (ctx.CurMillOperation == null) {
                ctx.WriteError("Операция фрезерования при поднятом шпинделе");
            } else {
                var part = new LinearMillPart(coordinate.Value);
                ctx.CurMillOperation.MillParts.Add(part);
            }
            ctx.CurCoordinate = coordinate.Value;
        }
    }
}