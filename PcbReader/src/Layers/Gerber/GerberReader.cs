using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Handlers;
using FormatSpecificationHandler = PcbReader.Layers.Gerber.Handlers.FormatSpecificationHandler;
using MoveOperationHandler = PcbReader.Layers.Gerber.Handlers.MoveOperationHandler;

namespace PcbReader.Layers.Gerber;

public class GerberReader: CommandReader<GerberCommandType, GerberContext, GerberLayer> {

    public static readonly GerberReader Instance = new();
    
    private static Dictionary<GerberCommandType, ICommandHandler<GerberCommandType, GerberContext, GerberLayer>> GetHandlers() {
        var handlers = new Dictionary<GerberCommandType, ICommandHandler<GerberCommandType, GerberContext, GerberLayer>> {
            { GerberCommandType.LineSegmentOperation, new CommandSegmentOperationHandler() },
            { GerberCommandType.Comment, new CommentCommandHandler() },
            { GerberCommandType.FormatSpecification, new FormatSpecificationHandler() },
            { GerberCommandType.MoveOperation, new MoveOperationHandler() },
            { GerberCommandType.SetLcMode, new SetLcModeHandler() },
            { GerberCommandType.SetCoordinates, new SetCoordinateModeHandler() },
            { GerberCommandType.Ignored, new IgnoredHandler() },
            { GerberCommandType.SetUom, new SetUomFormatHandler() },
            { GerberCommandType.DefineAperture, new DefineApertureHandler() },
            { GerberCommandType.SetAperture , new SetApertureHandler() },
            { GerberCommandType.ArcSegmentOperation , new ArcSegmentOperationHandler() },
            { GerberCommandType.DefineMacroAperture, new DefineMacroApertureHandler() },
            { GerberCommandType.FlashOperation, new FlashOperationHandler() }
        };
        return handlers;
    }
    
    private GerberReader():base(GetHandlers(),[]){ }
    
    protected override IEnumerable<string> ExcludeCommands(string text) {
        return text.Split(["\n","\r","%","*"],StringSplitOptions.RemoveEmptyEntries).Where(str => str!="");
    }
}