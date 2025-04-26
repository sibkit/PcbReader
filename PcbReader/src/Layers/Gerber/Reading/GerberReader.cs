using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Reading.CommandReaders;

namespace PcbReader.Layers.Gerber.Reading;

public class GerberReader: CommandsFileReader<GerberCommandType, GerberReadingContext, GerberLayer> {

    public static readonly GerberReader Instance = new();
    
    private static Dictionary<GerberCommandType, ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer>> GetHandlers() {
        var handlers = new Dictionary<GerberCommandType, ICommandReader<GerberCommandType, GerberReadingContext, GerberLayer>> {
            { GerberCommandType.LineSegmentOperation, new LineSegmentOperationReader() },
            { GerberCommandType.Comment, new CommentReader() },
            { GerberCommandType.FormatSpecification, new FormatSpecificationCommandReader() },
            { GerberCommandType.MoveOperation, new MoveOperationCommandReader() },
            { GerberCommandType.SetLcMode, new SetLcModeCommandReader() },
            { GerberCommandType.SetCoordinates, new SetCoordinateModeCommandReader() },
            { GerberCommandType.Ignored, new IgnoredCommandReader() },
            { GerberCommandType.SetUom, new SetUomFormatCommandReader() },
            { GerberCommandType.DefineAperture, new DefineApertureCommandReader() },
            { GerberCommandType.SetAperture , new SetApertureCommandReader() },
            { GerberCommandType.ArcSegmentOperation , new ArcSegmentOperationCommandReader() },
            { GerberCommandType.DefineApertureMacro, new DefineApertureTemplateCommandReader() },
            { GerberCommandType.FlashOperation, new FlashOperationCommandReader() },
            { GerberCommandType.BeginRegion , new BeginRegionCommandReader() },
            { GerberCommandType.EndRegion , new EndRegionCommandReader() },
        };
        return handlers;
    }
    
    private GerberReader():base(GetHandlers(),[]){ }
    
    protected override IEnumerable<string> ExcludeCommands(TextReader reader) {
        var ecOpened = false;
        string? extendedCommand = null;
        while (reader.ReadLine() is { } line) {
            var prevIndex = 0;
            for (var i = 0; i < line.Length; i++) {
                switch (line[i]) {
                    case '*':
                        if (prevIndex != i) {
                            if (extendedCommand == null) {
                                yield return line[prevIndex..(i + 1)];
                            } else {
                                extendedCommand += line[prevIndex..(i + 1)];
                            }
                        }

                        prevIndex = i + 1;
                        break;
                    case '%':
                        ecOpened = !ecOpened;

                        if (ecOpened) {
                            if (line.Length >= i + 3 && line[(i + 1)..(i + 3)] == "AM")
                                extendedCommand = "%";
                        } else if (extendedCommand != null) {
                            extendedCommand += line[i];
                            yield return extendedCommand;
                            extendedCommand = null;
                        }

                        prevIndex = i + 1;
                        break;
                }
            }
        }
    }
}