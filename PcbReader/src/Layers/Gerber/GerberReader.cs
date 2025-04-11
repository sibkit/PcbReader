using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Handlers;
using FormatSpecificationHandler = PcbReader.Layers.Gerber.Handlers.FormatSpecificationHandler;
using LineSegmentOperationHandler = PcbReader.Layers.Gerber.Handlers.LineSegmentOperationHandler;
using MoveOperationHandler = PcbReader.Layers.Gerber.Handlers.MoveOperationHandler;

namespace PcbReader.Layers.Gerber;

public class GerberReader: LineReader<GerberLineType, GerberContext, GerberLayer> {

    public static readonly GerberReader Instance = new();
    
    private static Dictionary<GerberLineType, ILineHandler<GerberLineType, GerberContext, GerberLayer>> GetHandlers() {
        var handlers = new Dictionary<GerberLineType, ILineHandler<GerberLineType, GerberContext, GerberLayer>> {
            { GerberLineType.LineSegmentOperation, new LineSegmentOperationHandler() },
            { GerberLineType.Comment, new CommentLineHandler() },
            { GerberLineType.FormatSpecification, new FormatSpecificationHandler() },
            { GerberLineType.MoveOperation, new MoveOperationHandler() },
            { GerberLineType.SetLcMode, new SetLcModeHandler() },
            { GerberLineType.SetCoordinatesModeHandler, new SetCoordinateModeHandler() },
            { GerberLineType.Ignored, new IgnoredHandler() },
            { GerberLineType.SetUom, new SetUomFormatHandler() },
            { GerberLineType.DefineAperture, new DefineApertureHandler() },
            { GerberLineType.SetApertureHandler , new SetApertureHandler() },
            { GerberLineType.ArcSegmentOperation , new ArcSegmentOperationHandler() },
            { GerberLineType.DefineMacroApertureHandler, new DefineMacroApertureHandler() },
            { GerberLineType.FlashOperation, new FlashOperationHandler() }
        };
        return handlers;
    }
    
    private GerberReader():base(GetHandlers(),[]){ }
    
    protected override IEnumerable<string> ExcludeLines(string text) {
        return text.Split(["\n","\r","%","*"],StringSplitOptions.RemoveEmptyEntries).Where(str => str!="");
    }
}