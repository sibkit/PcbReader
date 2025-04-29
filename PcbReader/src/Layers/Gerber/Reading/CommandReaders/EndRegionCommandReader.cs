﻿using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public class EndRegionCommandReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberReadingContext ctx) {
        return ctx.CurLine == "G37*";
    }
    public void WriteToProgram(GerberReadingContext ctx, GerberLayer layer) {
        if (ctx.CurPathPaintOperation != null) {
            ctx.CurPathPaintOperation.IsClosed = true;
            layer.Operations.Add(ctx.CurPathPaintOperation);
            ctx.CurPathPaintOperation = null;
        } else {
            ctx.WriteError("G37 Нет операций");
        }
    }
}