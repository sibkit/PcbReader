﻿using System.Text.RegularExpressions;
using PcbReader.Layers.Common;
using PcbReader.Layers.Common.Reading;

namespace PcbReader.Layers.Excellon.Handlers;

public partial class SetUomFormatReader : ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer> {
    public ExcellonCommandType[] GetNextLikelyTypes() {
        return [ExcellonCommandType.Comment];
    }

    public bool Match(ExcellonReadingContext ctx) {
        return ctx.CurLine.StartsWith("INCH") || ctx.CurLine.StartsWith("METRIC");
    }

    private static readonly Regex ReNum = NumberRegex();
    
    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer) {
        var lineParts = ctx.CurLine.Split(',');
        var firstPart = lineParts[0].Trim();

        layer.Uom = firstPart switch {
            "INCH" => Uom.Inch,
            "METRIC" => Uom.Metric,
            _ => throw new Exception("Invalid Uom")
        };

        ctx.Uom = layer.Uom;
        
        switch (lineParts.Length) {
            case 1:
                break;
            case 2: {
                var secondPart = lineParts[1].Trim();
                if (secondPart.Equals("LZ") ||
                    secondPart.Equals("TZ")) {
                    
                    var nz = secondPart switch {
                        "LZ" => Zeros.Leading,
                        "TZ" => Zeros.Trailing,
                        _ => throw new Exception("Invalid Zeroes")
                    };
                    var nf = ctx.NumberFormat;
                    if (nf.Zeros == null) {
                        nf.Zeros = nz;
                    } else if (nf.Zeros != nz) {
                        throw new Exception("SetUomFormatHandler: WriteToProgram expects zeros.");
                    }
                } else if (ReNum.IsMatch(secondPart)) {
                    var parts = secondPart.Split('.');

                    var left = parts[0].Length;
                    var right = parts[1].Length;
                    
                    if(ctx.NumberFormat.Left != null && ctx.NumberFormat.Left!=left)
                        throw new Exception("SetUomFormatHandler: WriteToProgram expects left.");
                    ctx.NumberFormat.Left = left;
                    if(ctx.NumberFormat.Right != null && ctx.NumberFormat.Right!=right)
                        throw new Exception("SetUomFormatHandler: WriteToProgram expects right.");
                    ctx.NumberFormat.Right = right;
                }

                break;
            }
            default:
                throw new Exception("LineParts.Length > 2");
        }
    }

    [GeneratedRegex(@"^\d+\.\d+$")]
    private static partial Regex NumberRegex();
}