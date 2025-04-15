using System.Globalization;
using System.Text.RegularExpressions;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Entities.Apertures;
using PcbReader.Layers.Gerber.Entities.MacroApertures;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public partial class DefineStandardApertureReader: ICommandHandler<GerberCommandType, GerberContext, GerberLayer>  {
    
    [GeneratedRegex(@"^ADD([0-9]+)([^,]*)\*$")]
    private static partial Regex MatchMacroRegex();
    
    [GeneratedRegex(@"^ADD([0-9]+)([CROP]{1}),{1}(.*)\*$")]
    private static partial Regex MatchRegex();
    private static readonly IFormatProvider Formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
    
    [GeneratedRegex("^([0-9.]+)(?:(?: *X *)([0-9.]+))*$")]
    private static partial Regex ParamsRegex();
    private static List<decimal?> ParseParams(string input) {
        var m = ParamsRegex().Match(input);
        var result = new List<decimal?>();
        for (var i = 1; i < m.Groups.Count; i++) {
            if(m.Groups[i].Value == "")
                result.Add(null);
            else
                result.Add(decimal.Parse(m.Groups[i].Value, Formatter));
        }
        return result;
    }

    private static CircleAperture ParseCircleAperture(string sParams) {
        var prs = ParseParams(sParams);
        return prs.Count switch {
            1 => new CircleAperture {
                Diameter = prs[0]!.Value,
            },
            2 => new CircleAperture {
                Diameter = prs[0]!.Value,
                HoleDiameter = prs[1]
            },
            _ => throw new Exception("DefineApertureHandler::ParseRectangleAperture(): Invalid string")
        };
    }

    private static RectangleAperture ParseRectangleAperture(string sParams) {
        var prs = ParseParams(sParams);
        return prs.Count switch {
            2 => new RectangleAperture {
                XSize = prs[0]!.Value,
                YSize = prs[1]!.Value,
            },
            3 => new RectangleAperture {
                XSize = prs[0]!.Value,
                YSize = prs[1]!.Value,
                HoleDiameter = prs[2]
            },
            _ => throw new Exception("DefineApertureHandler::ParseRectangleAperture(): Invalid string")
        };
    }
    
    private static ObRoundAperture ParseObRoundAperture(string sParams) {
        var prs = ParseParams(sParams);
        return prs.Count switch {
            2 => new ObRoundAperture {
                XSize = prs[0]!.Value,
                YSize = prs[1]!.Value,
            },
            3 => new ObRoundAperture {
                XSize = prs[0]!.Value,
                YSize = prs[1]!.Value,
                HoleDiameter = prs[2]
            },
            _ => throw new Exception("DefineApertureHandler::ParseObRoundAperture(): Invalid string")
        };
    }
    
    private static PolygonAperture ParsePolygonAperture(string sParams) {
        var prs = ParseParams(sParams);
        var outerDiameter = prs[0]!.Value;
        var verticesCount = (int)prs[1]!.Value;
        decimal? rotation = null;
        if (sParams.Length > 2) {
            rotation = prs[2];
        }

        decimal? holeDiameter = null;
        if (sParams.Length == 4) {
            holeDiameter = prs[3];
        }

        return new PolygonAperture {
            OuterDiameter = outerDiameter,
            VerticesCount = verticesCount,
            Rotation = rotation,
            HoleDiameter = holeDiameter
        };
    }
    
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    } 
    
    public bool Match(GerberContext ctx) {
        return ctx.CurLine.StartsWith("ADD");
    }
    
    public void WriteToProgram(GerberContext ctx, GerberLayer program) {
        
        var mm = MatchMacroRegex().Match(ctx.CurLine);
        if (mm.Success) {
            var code = int.Parse(mm.Groups[1].Value);
            var appName = mm.Groups[2].Value;
            program.Apertures.Add(code, new MacroAperture(appName));
            return;
        }
        
        var m = MatchRegex().Match(ctx.CurLine);
        if (m.Success) {
            var appNum = int.Parse(m.Groups[1].Value);
            var sParams = m.Groups[3].Value;
            switch (m.Groups[2].Value) {
                case "C":
                    program.Apertures.Add(appNum, ParseCircleAperture(sParams));
                break;
                case "R":
                    program.Apertures.Add(appNum, ParseRectangleAperture(sParams));
                    break;
                case "O":
                    program.Apertures.Add(appNum, ParseObRoundAperture(sParams));
                    break;
                case "P":
                    program.Apertures.Add(appNum, ParsePolygonAperture(sParams));
                    break;
                default:
                    throw new Exception("DefineApertureHandler: WriteToProgram (Unknown aperture type)");
            }
        } else {
            throw new Exception("DefineApertureHandler: WriteToProgram (Invalid match)");
        }
    }


}