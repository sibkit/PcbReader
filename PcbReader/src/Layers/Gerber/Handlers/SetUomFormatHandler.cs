using PcbReader.Layers.Gerber.Entities;
using PcbReader.Project;
using Exception = System.Exception;

namespace PcbReader.Layers.Gerber.Handlers;

public class SetUomFormatHandler: ILineHandler<GerberLineType, GerberContext, GerberLayer> {
    public GerberLineType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberContext ctx) {
        return ctx.CurLine is "MOIN" or "MOMM";
    }
    public void WriteToProgram(GerberContext ctx, GerberLayer program) {
        program.Uom = ctx.CurLine switch {
            "MOIN" => Uom.Inch,
            "MOMM" => Uom.Metric,
            _ => throw new Exception("Unknown UOM")
        };
    }
}