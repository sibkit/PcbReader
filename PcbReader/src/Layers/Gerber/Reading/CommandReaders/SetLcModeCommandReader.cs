using PcbReader.Layers.Common;
using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public class SetLcModeCommandReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return[];
    }
    public bool Match(GerberReadingContext ctx) {
        return ctx.CurLine is "G01*" or "G02*" or "G03*";
    }
    public void WriteToProgram(GerberReadingContext ctx, GerberLayer program) {
        ctx.LcMode = ctx.CurLine switch {
            "G01*" => LcMode.Linear,
            "G02*" => LcMode.Clockwise,
            "G03*" => LcMode.Counterclockwise,
            _ => throw new Exception("Unknown LC mode")
        };
    }
}