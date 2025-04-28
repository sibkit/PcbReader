﻿using PcbReader.Layers.Common.Reading;

namespace PcbReader.Layers.Excellon.Handlers;

public class CommentReader: ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.Comment];
    }
    public bool Match(ExcellonReadingContext ctx) {
        return ctx.CurLine.StartsWith(';') || ctx.CurLine.StartsWith("M47");
    }
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        var lines = ctx.CurLine.Split('=');
        if (lines.Length != 2) {
            ctx.WriteInfo("Комментарий: "+ctx.CurLine);
            return;
        }

        if (lines[0].Contains("FORMAT", StringComparison.OrdinalIgnoreCase)) {
            var fLines = lines[1].Split(':');
            if (fLines.Length == 2) {
                try {
                    var num1 = int.Parse(fLines[0]);
                    var num2 = int.Parse(fLines[1]);
                    ctx.NumberFormat.Left = num1;
                    ctx.NumberFormat.Right = num2;
                } catch (FormatException) {}
            }
        }
        ctx.WriteInfo("Комментарий: "+ctx.CurLine);
    }
}