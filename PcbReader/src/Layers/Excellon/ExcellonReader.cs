using PcbReader.Layers.Excellon.Handlers;
using SetToolHandler = PcbReader.Layers.Excellon.Handlers.SetToolHandler;
using SetUomFormatHandler = PcbReader.Layers.Excellon.Handlers.SetUomFormatHandler;

namespace PcbReader.Layers.Excellon;

public class ExcellonReader: LineReader<ExcellonLineType,ExcellonContext, ExcellonLayer> {
    
    public static readonly ExcellonReader Instance = new();

    private static Dictionary<ExcellonLineType, ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer>> GetHandlers() {
        var handlers = new Dictionary<ExcellonLineType, ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer>> {
            { ExcellonLineType.StartHeader, new StartHeaderHandler() },
            { ExcellonLineType.Comment, new CommentHandler() },
            { ExcellonLineType.EndHeader, new EndHeaderHandler() },
            { ExcellonLineType.SetUomFormat, new SetUomFormatHandler() },
            { ExcellonLineType.SetTool, new SetToolHandler() },
            { ExcellonLineType.DrillOperation, new DrillingOperationHandler() },
            { ExcellonLineType.ToolDefine, new ToolDefineHandler() },
            { ExcellonLineType.Ignored, new IgnoredFormatHandler() },
            { ExcellonLineType.BeginPattern, new BeginPatternHandler() },
            { ExcellonLineType.EndPattern, new EndPatternHandler() },
            { ExcellonLineType.RepeatPattern, new RepeatPatternHandler() },
            { ExcellonLineType.SetDrillMode, new SetDrillModeHandler() },
            { ExcellonLineType.EndProgram, new EndProgramHandler() },
            { ExcellonLineType.SetCoordinatesMode, new SetCoordinatesModeHandler() },
            { ExcellonLineType.RoutOperation, new RoutOperationHandler() },
            { ExcellonLineType.StartMill, new StartMillHandler() },
            { ExcellonLineType.EndMill, new EndMillHandler() },
            { ExcellonLineType.LinearMillOperation, new LinearMillOperationHandler() },
            { ExcellonLineType.ArcMillOperation, new ArcMillOperationHandler() }
        };
        return handlers;
    }
    
    private ExcellonReader():base(GetHandlers(),[ExcellonLineType.StartHeader]){ }
    protected override IEnumerable<string> ExcludeLines(string text) {
        return text.Split('\n','\r').Where(str => str!="");
    }
}