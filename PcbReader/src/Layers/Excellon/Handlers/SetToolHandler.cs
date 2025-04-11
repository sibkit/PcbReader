using System.Text.RegularExpressions;

namespace PcbReader.Layers.Excellon.Handlers;

public partial class SetToolHandler: ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer> {
    
    private static readonly Regex ReSetTool = SetToolRegex();
    
    public ExcellonLineType[] GetNextLikelyTypes() {
        return [ExcellonLineType.DrillOperation];
    }
    public bool Match(ExcellonContext ctx) {
        return ReSetTool.IsMatch(ctx.CurLine);
    }
    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer) {
        var toolNumber = int.Parse(ctx.CurLine[1..]);
        ctx.CurToolNumber = toolNumber;
    }
    
    [GeneratedRegex(@"^T\d+$")]
    private static partial Regex SetToolRegex();
}

