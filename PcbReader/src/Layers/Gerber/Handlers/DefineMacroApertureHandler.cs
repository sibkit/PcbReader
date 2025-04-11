using System.Text.RegularExpressions;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Entities.Apertures;

namespace PcbReader.Layers.Gerber.Handlers;

public partial class DefineMacroApertureHandler: ILineHandler<GerberLineType, GerberContext, GerberLayer>  {
    
    [GeneratedRegex(@"^^ADD([0-9]+)([^,]*)$")]
    private static partial Regex MatchRegex();
    
    public GerberLineType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberContext ctx) {
        return MatchRegex().IsMatch(ctx.CurLine);
    }
    public void WriteToProgram(GerberContext ctx, GerberLayer program) {
        var m = MatchRegex().Match(ctx.CurLine);
        var code = int.Parse(m.Groups[1].Value);
        var appName = m.Groups[2].Value;
        program.Apertures.Add(code, new MacroAperture(appName));
    }
}