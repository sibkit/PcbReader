using System.Text.RegularExpressions;
using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Handlers;

public partial class FlashOperationHandler: ICommandHandler<GerberCommandType, GerberContext, GerberLayer>  {
    
    [GeneratedRegex("^(?:(X)([+-]?[0-9.]+))?(?:(Y)([+-]?[0-9.]+))?D03$")]
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

        if (ctx.CurApertureCode == null) {
            ctx.WriteError("Не задана апертура");
            return;
        }

        program.Operations.Add(new FlashOperation {
            Coordinate = Coordinates.ParseCoordinate(ctx.NumberFormat!,xs,ys),
            ApertureCode = ctx.CurApertureCode.Value
        });
    }
}