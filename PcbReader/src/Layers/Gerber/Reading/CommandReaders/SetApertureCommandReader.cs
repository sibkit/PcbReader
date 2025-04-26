using System.Text.RegularExpressions;
using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public partial class SetApertureCommandReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer> {

    [GeneratedRegex("^(?:G54)*D([0-9]+)\\*$")]
    private static partial Regex MyRegex();
    
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberReadingContext ctx) {
        return MyRegex().IsMatch(ctx.CurLine);
    }
    public void WriteToProgram(GerberReadingContext ctx, GerberLayer program) {
        var m = MyRegex().Match(ctx.CurLine);
        var apCode = int.Parse(m.Groups[1].Value);
        if (program.Apertures.ContainsKey(apCode)) {
            ctx.CurApertureCode = apCode;
        } else {
            ctx.WriteError("Не найдена аппертура с кодом: "+apCode);
        }
    }
}