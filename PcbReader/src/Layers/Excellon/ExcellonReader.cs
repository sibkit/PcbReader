using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Excellon.CommandReaders;
using PcbReader.Layers.Excellon.Entities;
using ArcMillOperationReader = PcbReader.Layers.Excellon.CommandReaders.ArcMillOperationReader;
using SetToolReader = PcbReader.Layers.Excellon.CommandReaders.SetToolReader;
using SetUomFormatReader = PcbReader.Layers.Excellon.CommandReaders.SetUomFormatReader;
using ToolDefineReader = PcbReader.Layers.Excellon.CommandReaders.ToolDefineReader;

namespace PcbReader.Layers.Excellon;

public class ExcellonReader: CommandsFileReader<ExcellonCommandType,ExcellonReadingContext, ExcellonLayer> {
    
    public static readonly ExcellonReader Instance = new();

    private static Dictionary<ExcellonCommandType, ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer>> GetHandlers() {
        var handlers = new Dictionary<ExcellonCommandType, ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer>> {
            { ExcellonCommandType.StartHeader, new StartHeaderReader() },
            { ExcellonCommandType.Comment, new CommentReader() },
            { ExcellonCommandType.EndHeader, new EndHeaderReader() },
            { ExcellonCommandType.SetUomFormat, new SetUomFormatReader() },
            { ExcellonCommandType.SetTool, new SetToolReader() },
            { ExcellonCommandType.DrillOperation, new DrillingOperationReader() },
            { ExcellonCommandType.ToolDefine, new ToolDefineReader() },
            { ExcellonCommandType.Ignored, new IgnoredFormatReader() },
            { ExcellonCommandType.BeginPattern, new BeginPatternReader() },
            { ExcellonCommandType.EndPattern, new EndPatternReader() },
            { ExcellonCommandType.RepeatPattern, new RepeatPatternReader() },
            { ExcellonCommandType.SetDrillMode, new SetDrillModeReader() },
            { ExcellonCommandType.EndProgram, new EndProgramCommandReader() },
            { ExcellonCommandType.SetCoordinatesMode, new SetCoordinatesModeReader() },
            { ExcellonCommandType.RoutOperation, new RoutOperationReader() },
            { ExcellonCommandType.StartMill, new StartMillReader() },
            { ExcellonCommandType.EndMill, new EndMillReader() },
            { ExcellonCommandType.LinearMillOperation, new LinearMillOperationReader() },
            { ExcellonCommandType.ArcMillOperation, new ArcMillOperationReader() }
        };
        return handlers;
    }
    
    private ExcellonReader():base(GetHandlers(),[ExcellonCommandType.StartHeader]){ }
    protected override IEnumerable<string> ExcludeCommands(TextReader reader) {
        return reader.ReadToEnd().Split('\n','\r').Where(str => str!="");
    }
}