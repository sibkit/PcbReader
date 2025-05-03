using System.Text.RegularExpressions;
using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Excellon.Entities;

namespace PcbReader.Layers.Excellon.CommandReaders;

public partial class SetToolReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    
    private static readonly Regex ReSetTool = SetToolRegex();
    
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.DrillOperation];
    }
    public bool Match(ExcellonReadingContext ctx) {
        return ReSetTool.IsMatch(ctx.CurLine);
    }
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        var toolNumber = int.Parse(ctx.CurLine[1..]);
        ctx.CurToolNumber = toolNumber;
    }
    
    [GeneratedRegex(@"^T\d+$")]
    private static partial Regex SetToolRegex();
}

