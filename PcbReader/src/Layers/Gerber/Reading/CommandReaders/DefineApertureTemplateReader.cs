using System.Text.RegularExpressions;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Entities.MacroApertures;
using PcbReader.Layers.Gerber.Macro.Expressions;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public partial class DefineApertureTemplateReader: ICommandHandler<GerberCommandType, GerberContext, GerberLayer>  {
    
    [GeneratedRegex(@"^%AM.+%$")]
    private static partial Regex MatchRegex();
    
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberContext ctx) {
        return MatchRegex().IsMatch(ctx.CurLine);
    }

    private static LinkExpression ReadParameter(string line) {
        var parts = line.Split("=", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length != 2) {
            throw new ApplicationException("Invalid parameter format");
        }
        var result = new LinkExpression {
            Name = parts[0]
        };
        //result.Value = ParseP
        return result;
    }
    
    public void WriteToProgram(GerberContext ctx, GerberLayer program) {
        var lines = ctx.CurLine.Trim('%').Split('*', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var am = new MacroAperture(lines[0][2..]);
        

        foreach (var line in lines[1..]) {
            if (line.StartsWith('$')) {
                am.Expressions.Add(ReadParameter(line));
            }
        }
        
        Console.WriteLine(ctx.CurLine);
        //program.Apertures.Add(code, new MacroAperture(appName));
    }
}