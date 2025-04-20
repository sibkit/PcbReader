﻿using System.Text.RegularExpressions;
using PcbReader.Layers.Common;
using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Entities.StandardApertures;
using PcbReader.Project;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public partial class ArcSegmentOperationReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer> {
    
    [GeneratedRegex("^(G02|G03){0,1}(?:(X)([+-]?[0-9.]+))(?:(Y)([+-]?[0-9.]+))(?:(I)([+-]?[0-9.]+))(?:(J)([+-]?[0-9.]+))D01\\*$")]
    private static partial Regex MatchRegex();
    
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberReadingContext ctx) {
        return MatchRegex().IsMatch(ctx.CurLine);
    }
    public void WriteToProgram(GerberReadingContext ctx, GerberLayer program) {
        var m = MatchRegex().Match(ctx.CurLine);

        var shift = 0;
        switch (m.Groups[1].Value) {
            case "G02":
                shift = 1;
                ctx.LcMode = LcMode.Clockwise;
                break;
            case "G03":
                shift = 1;
                ctx.LcMode = LcMode.Counterclockwise;
                break;
        }
        
        var xs = m.Groups[2+shift].Value;
        var ys = m.Groups[4+shift].Value;

        var si = m.Groups[6+shift].Value;
        var sj = m.Groups[8+shift].Value;
        
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
                ctx.CurPathPaintOperation ??= new PathPaintOperation(ca, (Point)ctx.CurCoordinate);

                var part = new ArcPathPart {
                    EndPoint = c,
                    IOffset = Coordinates.ReadValue(ctx.NumberFormat!,si),
                    JOffset = Coordinates.ReadValue(ctx.NumberFormat!,sj)
                };
                ctx.CurPathPaintOperation!.Parts.Add(part);

                break;
            case null:
                ctx.WriteError("Аппертура не задана");
                break;              
            default:
                ctx.WriteError("Неправильная аппертура: для операции D01 поддерживаются только круговые аппертуры");
                break;
        }
    }
}