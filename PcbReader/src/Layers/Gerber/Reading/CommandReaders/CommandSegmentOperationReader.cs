using System.Text.RegularExpressions;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Entities.Apertures;
using PcbReader.Project;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public partial class CommandSegmentOperationReader: ICommandHandler<GerberCommandType, GerberContext, GerberLayer> {

    [GeneratedRegex("^(?:(X)([+-]?[0-9.]+))?(?:(Y)([+-]?[0-9.]+))?D01\\*$")]
    private static partial Regex MatchRegex();
    
    public GerberCommandType[] GetNextLikelyTypes() {
        return [
            GerberCommandType.LineSegmentOperation, 
            //GerberLineType.ArcSegmentOperation
        ];
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
                ctx.CurPathPaintOperation ??= new PathPaintOperation(ca, (Coordinate)ctx.CurCoordinate);

                var part = new LinePathPart(c);
                ctx.CurPathPaintOperation!.Parts.Add(part);

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