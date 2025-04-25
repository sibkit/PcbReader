using System.Text.RegularExpressions;
using PcbReader.Layers.Common;
using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Entities.StandardApertures;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public partial class LineSegmentOperationReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer> {

    [GeneratedRegex("^(?:(X)([+-]?[0-9.]+))?(?:(Y)([+-]?[0-9.]+))?D01\\*$")]
    private static partial Regex MatchRegex();
    
    public GerberCommandType[] GetNextLikelyTypes() {
        return [
            GerberCommandType.LineSegmentOperation, 
            GerberCommandType.ArcSegmentOperation
        ];
    }
    public bool Match(GerberReadingContext ctx) {
        return MatchRegex().IsMatch(ctx.CurLine);
    }

    public void WriteToProgram(GerberReadingContext ctx, GerberLayer program) {
        var m = MatchRegex().Match(ctx.CurLine);
        
        var xs = m.Groups[2].Value;
        var ys = m.Groups[4].Value;

        if (ctx.NumberFormat == null) {
            ctx.WriteError("Не определен формат чисел.");
            ctx.ContinueHandle = false;
            return;
        }
        
        if (ctx.CurCoordinate == null) {
            ctx.WriteError("Не задана начальная координата");
            ctx.ContinueHandle = false;
            return;
        }
        
        var c = Coordinates.ParseCoordinate(ctx.NumberFormat!,xs,ys);
        if (ctx.CurApertureCode == null) {
            ctx.WriteError("Аппертура не задана");
            return;
        }
            
        var curAperture = program.Apertures[ctx.CurApertureCode.Value];
        switch (curAperture) {
            case CircleAperture ca:
                if (ctx.CurPathPaintOperation == null) {
                    var op = new PathPaintOperation(ca, (Point)ctx.CurCoordinate);
                    op.Parts.Add(new LinePathPart(c));
                    program.Operations.Add(op);
                } else {
                    var part = new LinePathPart(c);
                    ctx.CurPathPaintOperation!.Parts.Add(part);
                }
                break;
            case null:
                ctx.WriteError("Аппертура не задана");
                break;              
            default:
                ctx.WriteWarning("Неправильная аппертура: для операции типа D01 следует использовать только круговые аппертуры");
                break;
        }
    }
}