using PcbReader.Layers.Excellon.Entities;
using PcbReader.Project;

namespace PcbReader.Layers.Excellon.Handlers;

public class DrillingOperationHandler: ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer> {
    
    public ExcellonLineType[] GetNextLikelyTypes() {
        return[ExcellonLineType.DrillOperation, ExcellonLineType.SetTool];
    }
    public bool Match(ExcellonContext ctx) {
        return ExcellonCoordinates.IsCoordinate(ctx.CurLine);
    }
    
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        int? toolNumber;
        Coordinate? coordinate;
        
        if (ctx.CurToolNumber != null) 
            toolNumber = ctx.CurToolNumber.Value;
        else {
            throw new ApplicationException("Tool not defined");
        }
        var readedCoordinate = ExcellonCoordinates.ReadCoordinate(ctx.CurLine, ctx) ?? throw new Exception( "DrillingOperationHandler: WriteToProgram (line matched, not readed)");
        switch (ctx.CoordinatesMode) {
            case CoordinatesMode.Incremental:
                if (layer.Operations.Count != 0) {
                    var lastOperation = layer.Operations.Last();
                    coordinate = lastOperation.StartCoordinate + readedCoordinate;
                } else {
                    coordinate = readedCoordinate;
                }
                break;
            case CoordinatesMode.Absolute:
                coordinate = readedCoordinate;
                break;
            default:
                throw new Exception("DrillingOperationHandler: WriteToProgram (Unknown Coordinates mode)");
        }
        ctx.CurCoordinate = coordinate.Value;
        var result = new DrillOperation(coordinate!.Value, toolNumber!.Value);
        if (ctx.CurPattern is { State: PatternState.Opened }) {
            ctx.CurPattern.MachiningOperations.Add(result);
        }
        layer.Operations.Add(result);
    }
}