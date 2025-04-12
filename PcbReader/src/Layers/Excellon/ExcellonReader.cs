using PcbReader.Layers.Excellon.Handlers;
using SetToolHandler = PcbReader.Layers.Excellon.Handlers.SetToolHandler;
using SetUomFormatHandler = PcbReader.Layers.Excellon.Handlers.SetUomFormatHandler;

namespace PcbReader.Layers.Excellon;

public class ExcellonReader: CommandReader<ExcellonCommandType,ExcellonContext, ExcellonLayer> {
    
    public static readonly ExcellonReader Instance = new();

    private static Dictionary<ExcellonCommandType, ICommandHandler<ExcellonCommandType, ExcellonContext, ExcellonLayer>> GetHandlers() {
        var handlers = new Dictionary<ExcellonCommandType, ICommandHandler<ExcellonCommandType, ExcellonContext, ExcellonLayer>> {
            { ExcellonCommandType.StartHeader, new StartHeaderHandler() },
            { ExcellonCommandType.Comment, new CommentHandler() },
            { ExcellonCommandType.EndHeader, new EndHeaderHandler() },
            { ExcellonCommandType.SetUomFormat, new SetUomFormatHandler() },
            { ExcellonCommandType.SetTool, new SetToolHandler() },
            { ExcellonCommandType.DrillOperation, new DrillingOperationHandler() },
            { ExcellonCommandType.ToolDefine, new ToolDefineHandler() },
            { ExcellonCommandType.Ignored, new IgnoredFormatHandler() },
            { ExcellonCommandType.BeginPattern, new BeginPatternHandler() },
            { ExcellonCommandType.EndPattern, new EndPatternHandler() },
            { ExcellonCommandType.RepeatPattern, new RepeatPatternHandler() },
            { ExcellonCommandType.SetDrillMode, new SetDrillModeHandler() },
            { ExcellonCommandType.EndProgram, new EndProgramHandler() },
            { ExcellonCommandType.SetCoordinatesMode, new SetCoordinatesModeHandler() },
            { ExcellonCommandType.RoutOperation, new RoutOperationHandler() },
            { ExcellonCommandType.StartMill, new StartMillHandler() },
            { ExcellonCommandType.EndMill, new EndMillHandler() },
            { ExcellonCommandType.LinearMillOperation, new LinearMillOperationHandler() },
            { ExcellonCommandType.ArcMillOperation, new ArcMillOperationHandler() }
        };
        return handlers;
    }
    
    private ExcellonReader():base(GetHandlers(),[ExcellonCommandType.StartHeader]){ }
    protected override IEnumerable<string> ExcludeCommands(string text) {
        return text.Split('\n','\r').Where(str => str!="");
    }
}