using PcbReader.Layers.Gerber.Entities;

namespace PcbReader.Layers.Gerber.Reading.CommandReaders;

public class IgnoredReader: ICommandHandler<GerberCommandType, GerberContext, GerberLayer> {
    public GerberCommandType[] GetNextLikelyTypes() {
        return [];
    }
    public bool Match(GerberContext ctx) {
        return ctx.CurLine switch {
            "G75*" => true, //This command must be issued before the first circular plotting operation, for compatibility with older Gerber versions
            "G74*" => true, //This command must be issued before the first circular plotting operation, for compatibility with older Gerber versions
            "M00*" => true, //Program stop
            _ => false
        };
    }
    public void WriteToProgram(GerberContext ctx, GerberLayer program) {
        if (ctx.CurLine is "G74*" or "G75*") {
            ctx.WriteWarning("Устаревшая команда: "+ctx.CurLine);
        }
        ctx.WriteInfo("Пропущенная команда: " + ctx.CurLine);
    }
}