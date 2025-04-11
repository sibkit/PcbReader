using System.Globalization;
using System.Text.RegularExpressions;

namespace PcbReader.Layers.Excellon.Handlers;

public partial class ToolDefineHandler: ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer> {
    
    private static readonly Regex ReToolDefine = ToolDefineRegex();
    private readonly IFormatProvider _formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
    public ExcellonLineType[] GetNextLikelyTypes() {
        return[ExcellonLineType.ToolDefine, ExcellonLineType.EndHeader];
    }
    public bool Match(ExcellonContext ctx) {
        return ReToolDefine.IsMatch(ctx.CurLine);
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        var match = ReToolDefine.Match(ctx.CurLine);
        if (match.Groups.Count == 3) {
            var toolNum = int.Parse(match.Groups[1].Value);
            layer.ToolsMap.Add(toolNum, decimal.Parse(match.Groups[2].Value, _formatter));
        } else {
            throw new Exception("ToolDefineHandler.WriteToProgram: Invalid line.");
        }
    }
    
    [GeneratedRegex(@"T([0-9]+)[A-Z0-9]*C([0-9.]+)(?:[A-Z]*|$)")]
    private static partial Regex ToolDefineRegex();
}