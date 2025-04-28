﻿using System.Text.RegularExpressions;
using PcbReader.Layers.Common.Reading;

namespace PcbReader.Layers.Excellon.Handlers;

public partial class ArcMillOperationReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    
    private static readonly Regex ReArcMill = ArcMillRegex();
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [
            ExcellonCommandType.ArcMillOperation, 
            ExcellonCommandType.LinearMillOperation, 
            ExcellonCommandType.EndMill
        ];
    }
    public bool Match(ExcellonReadingContext ctx) {
        return ctx.CurLine.StartsWith("G02") || ctx.CurLine.StartsWith("G03");
    }
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        var match = ReArcMill.Match(ctx.CurLine);
        if (match.Success) {
            
            var gCode = match.Groups[1].Value;
            var coordinate = match.Groups[2].Value;
            var a = match.Groups[4].Value;
        } else {
            ctx.WriteError("Не удалось распознать строку: \"" + ctx.CurLine + "\"");
        }
    }

    
    [GeneratedRegex("^(G02|G03)((?:[XY][+-]?[0-9.]+)?(?:[XY][+-]?[0-9.]+))?(?:(A)([+-]?[0-9.]+))$")]
    private static partial Regex ArcMillRegex();
}