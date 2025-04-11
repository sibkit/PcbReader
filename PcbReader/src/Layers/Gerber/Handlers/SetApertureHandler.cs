using System.Text.RegularExpressions;
using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Handlers;

public partial class SetApertureHandler: ILineHandler<GerberLineType, GerberContext, GerberLayer> {

    [GeneratedRegex("^(?:G54)*D([0-9]+)$")]
    private static partial Regex MyRegex();
    
    public GerberLineType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberContext ctx) {
        return MyRegex().IsMatch(ctx.CurLine);
    }
    public void WriteToProgram(GerberContext ctx, GerberLayer program) {
        var m = MyRegex().Match(ctx.CurLine);
        var apCode = int.Parse(m.Groups[1].Value);
        if (program.Apertures.ContainsKey(apCode)) {
            ctx.CurApertureCode = apCode;
        } else {
            ctx.WriteError("Не найдена аппертура с кодом: "+apCode);
        }
    }
}