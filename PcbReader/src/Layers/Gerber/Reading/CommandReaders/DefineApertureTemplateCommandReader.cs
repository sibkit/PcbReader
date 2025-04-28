using System.Text.RegularExpressions;
using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Entities.Apertures.Macro;
using PcbReader.Layers.Gerber.Entities.Apertures.Macro.Expressions;
using PcbReader.Layers.Gerber.Entities.Apertures.Macro.Primitives;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public partial class DefineApertureTemplateCommandReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer>  {
    
    [GeneratedRegex(@"^%AM.+%$")]
    private static partial Regex MatchRegex();
    
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberReadingContext ctx) {
        return MatchRegex().IsMatch(ctx.CurLine);
    }

    private static ParameterExpression ReadParameter(string line) {
        var parts = line.Split("=", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length != 2) {
            throw new ApplicationException("Invalid parameter format");
        }
        var result = new ParameterExpression(parts[0]);
        //result.Value = ParseP
        return result;
    }

    public static IPrimitive ReadPrimitive(string line) {
        return line[0] switch {
            '1' => ReadCircle(line),
            _ => throw new ApplicationException("Invalid primitive code")
        };
    }
    
    public void WriteToProgram(GerberReadingContext ctx, GerberLayer program) {
        var lines = ctx.CurLine.Trim('%').Split('*', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var am = new MacroApertureTemplate(lines[0][2..]);
        

        foreach (var line in lines[1..]) {
            if (line.StartsWith('$')) {
                am.Expressions.Add(ReadParameter(line));
            } else if(line.StartsWith('0')){
                am.Comments.Add(line);
            } else {
                am.Primitives.Add(ReadPrimitive(line));
            }
        }
        
        Console.WriteLine(ctx.CurLine);
        //program.Apertures.Add(code, new MacroAperture(appName));
    }

    private static IExpression ReadExpression(string eValue) {
        if (eValue.StartsWith('$')) 
            return new ParameterExpression(eValue);
        try {
            var expVal = double.Parse(eValue);
            return new ValueExpression(expVal);
        } catch {
            throw new ApplicationException("Invalid exposure format");
        }
    }
    
    
    
    private static Circle ReadCircle(string line) {
        var values = line.Split(',');
        var result = new Circle {
            Exposure = ReadExpression(values[0])
        };
        return result;
    }
    
}