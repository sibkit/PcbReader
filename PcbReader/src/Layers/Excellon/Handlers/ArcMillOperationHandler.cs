using System.Text.RegularExpressions;

namespace PcbReader.Layers.Excellon.Handlers;

public partial class ArcMillOperationHandler: ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer> {
    
    private static readonly Regex ReArcMill = ArcMillRegex();
    public ExcellonLineType[] GetNextLikelyTypes() {
        return [
            ExcellonLineType.ArcMillOperation, 
            ExcellonLineType.LinearMillOperation, 
            ExcellonLineType.EndMill
        ];
    }
    public bool Match(ExcellonContext ctx) {
        return ctx.CurLine.StartsWith("G02") || ctx.CurLine.StartsWith("G03");
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        var match = ReArcMill.Match(ctx.CurLine);
        if (match.Success) {
            
            var gCode = match.Groups[1].Value;
            var coordinate = match.Groups[2].Value;
            var a = match.Groups[4].Value;
        } else {
            ctx.WriteError("Не удалось распознать строку: \"" + ctx.CurLine + "\"");
        }
    }

    
    [GeneratedRegex("^(G02|G03)((?:[XY][+-]?[0-9.]+)?(?:[XY][+-]?[0-9.]+))?(?:(A)([+-]?[0-9.]+))$")]
    private static partial Regex ArcMillRegex();
}