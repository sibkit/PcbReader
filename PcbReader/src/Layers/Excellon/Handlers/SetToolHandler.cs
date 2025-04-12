using System.Text.RegularExpressions;

namespace PcbReader.Layers.Excellon.Handlers;

public partial class SetToolHandler: ICommandHandler<ExcellonCommandType, ExcellonContext, ExcellonLayer> {
    
    private static readonly Regex ReSetTool = SetToolRegex();
    
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.DrillOperation];
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

