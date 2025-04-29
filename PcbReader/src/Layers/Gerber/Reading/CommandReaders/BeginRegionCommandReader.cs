﻿using PcbReader.Geometry;
using PcbReader.Layers.Common;
using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Entities.Apertures;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public class BeginRegionCommandReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberReadingContext ctx) {
        return ctx.CurLine == "G36*";
    }
    public void WriteToProgram(GerberReadingContext ctx, GerberLayer layer) {
        
        if (ctx.CurApertureCode == null) {
            ctx.WriteError("Не задана аппертура перед командой G36");
            return;
        }
            
        var curAperture = layer.Apertures[ctx.CurApertureCode.Value];
        
        switch (curAperture) {
            case CircleAperture ca:
                if (ctx.CurCoordinate == null) {
                    ctx.WriteError("Не задана начальная координата перед командой G36");
                }
                ctx.CurPathPaintOperation ??= new PathPaintOperation(ca, (Point)ctx.CurCoordinate!);
                
                break;
            case null:
                ctx.WriteError("Не задана аппертура перед командой G36");
                break;              
            default:
                ctx.WriteError("Неправильная аппертура: для операции D01 поддерживаются только круговые аппертуры");
                break;
        }
    }
}