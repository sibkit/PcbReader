using System.Text.RegularExpressions;
using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public partial class MoveOperationReader: ICommandHandler<GerberCommandType, GerberContext, GerberLayer> {
    
    [GeneratedRegex("^(?:(X)([+-]?[0-9.]+))?(?:(Y)([+-]?[0-9.]+))?D02\\*$")]
    private static partial Regex MatchRegex();
    
    
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberContext ctx) {
        return MatchRegex().IsMatch(ctx.CurLine);
    }
    public void WriteToProgram(GerberContext ctx, GerberLayer program) {
        var m = MatchRegex().Match(ctx.CurLine);
        var xs = m.Groups[2].Value;
        var ys = m.Groups[4].Value;
        
        if (ctx.NumberFormat == null) {
            ctx.WriteError("Не определен формат чисел.");
            ctx.ContinueHandle = false;
            return;
        }
        
        ctx.CurCoordinate = Coordinates.ParseCoordinate(ctx.NumberFormat!,xs,ys);
    }
}