﻿using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public class IgnoredCommandReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberReadingContext ctx) {
        return ctx.CurLine switch {
            "G75*" => true, //This command must be issued before the first circular plotting operation, for compatibility with older Gerber versions
            "G74*" => true, //This command must be issued before the first circular plotting operation, for compatibility with older Gerber versions
            "M00*" => true, //Program stop
            _ => false
        };
    }
    public void WriteToProgram(GerberReadingContext ctx, GerberLayer layer) {
        if (ctx.CurLine is "G74*" or "G75*") {
            ctx.WriteWarning("Устаревшая команда: "+ctx.CurLine);
        }
        ctx.WriteInfo("Пропущенная команда: " + ctx.CurLine);
    }
}