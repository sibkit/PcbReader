﻿using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Excellon.Entities;

namespace PcbReader.Layers.Excellon.Handlers;

public class EndPatternReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.RepeatPattern];
    }
    public bool Match(ExcellonReadingContext ctx) {
        return ctx.CurLine is "M01" or "M08";
    }
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        switch (ctx.CurLine) {
            case "M01": {
                if (ctx.CurPattern == null || ctx.CurPattern.State == PatternState.Closed) {
                    ctx.WriteError("Команда закрытия шаблона, без его открытия");
                    return;
                }
                
                ctx.CurPattern.State = PatternState.Closed;
                break;
            }
            case "M08":
                ctx.CurPattern = null;
                break;
        }
    }
}