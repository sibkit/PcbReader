﻿using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Excellon.Entities;
using PcbReader.Project;
using ApplicationException = System.ApplicationException;

namespace PcbReader.Layers.Excellon.Handlers;

public class BeginPatternReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.DrillOperation];
    }
    public bool Match(ExcellonReadingContext ctx) {
        return ctx.CurLine == "M25";
    }
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        if (ctx.CurPattern == null || ctx.CurPattern.State == PatternState.Closed) {
            var coordinate = new Coordinate(0,0);
            if (layer.Operations.Count != 0) {
                var lo = layer.Operations.Last();
                coordinate = lo.StartCoordinate;
            }
            
            ctx.CurPattern = new Pattern(coordinate);
            
        } else {
            throw new ApplicationException("Команда открытия шаблона при уже открытом шаблоне.");
        }
    }
}