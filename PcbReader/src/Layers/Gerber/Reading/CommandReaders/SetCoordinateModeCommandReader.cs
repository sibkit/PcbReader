using PcbReader.Layers.Common;
using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public class SetCoordinateModeCommandReader: ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberReadingContext ctx) {
        return ctx.CurLine is "G90*" or "G91*";
    }
    public void WriteToProgram(GerberReadingContext ctx, GerberLayer layer) {
        ctx.CoordinatesMode = ctx.CurLine switch {
            "G90*" => CoordinatesMode.Absolute,
            "G91*" => CoordinatesMode.Incremental,
            _ => throw new Exception("Unknown coordinates mode")
        };
    }
}