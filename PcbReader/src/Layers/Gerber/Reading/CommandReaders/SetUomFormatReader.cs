using PcbReader.Layers.Common;
using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;
using Exception = System.Exception;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public class SetUomFormatReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberReadingContext ctx) {
        return ctx.CurLine is "MOIN*" or "MOMM*";
    }
    public void WriteToProgram(GerberReadingContext ctx, GerberLayer program) {
        program.Uom = ctx.CurLine switch {
            "MOIN*" => Uom.Inch,
            "MOMM*" => Uom.Metric,
            _ => throw new Exception("Unknown UOM")
        };
    }
}