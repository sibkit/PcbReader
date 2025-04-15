
using PcbReader.Layers.Gerber.Entities;
using PcbReader.Layers.Gerber.Reading.CommandReaders;
using ArcSegmentOperationReader = PcbReader.Layers.Gerber.Reading.CommandReaders.ArcSegmentOperationReader;
using CommandSegmentOperationReader = PcbReader.Layers.Gerber.Reading.CommandReaders.CommandSegmentOperationReader;
using DefineApertureTemplateReader = PcbReader.Layers.Gerber.Reading.CommandReaders.DefineApertureTemplateReader;
using DefineStandardApertureReader = PcbReader.Layers.Gerber.Reading.CommandReaders.DefineStandardApertureReader;
using FlashOperationReader = PcbReader.Layers.Gerber.Reading.CommandReaders.FlashOperationReader;
using FormatSpecificationReader = PcbReader.Layers.Gerber.Reading.CommandReaders.FormatSpecificationReader;
using MoveOperationReader = PcbReader.Layers.Gerber.Reading.CommandReaders.MoveOperationReader;
using SetApertureReader = PcbReader.Layers.Gerber.Reading.CommandReaders.SetApertureReader;

namespace PcbReader.Layers.Gerber;

public class GerberReader: CommandReader<GerberCommandType, GerberContext, GerberLayer> {

    public static readonly GerberReader Instance = new();
    
    private static Dictionary<GerberCommandType, ICommandHandler<GerberCommandType, GerberContext, GerberLayer>> GetHandlers() {
        var handlers = new Dictionary<GerberCommandType, ICommandHandler<GerberCommandType, GerberContext, GerberLayer>> {
            { GerberCommandType.LineSegmentOperation, new CommandSegmentOperationReader() },
            { GerberCommandType.Comment, new CommentReader() },
            { GerberCommandType.FormatSpecification, new FormatSpecificationReader() },
            { GerberCommandType.MoveOperation, new MoveOperationReader() },
            { GerberCommandType.SetLcMode, new SetLcModeReader() },
            { GerberCommandType.SetCoordinates, new SetCoordinateModeReader() },
            { GerberCommandType.Ignored, new IgnoredReader() },
            { GerberCommandType.SetUom, new SetUomFormatReader() },
            { GerberCommandType.DefineAperture, new DefineStandardApertureReader() },
            { GerberCommandType.SetAperture , new SetApertureReader() },
            { GerberCommandType.ArcSegmentOperation , new ArcSegmentOperationReader() },
            { GerberCommandType.DefineApertureMacro, new DefineApertureTemplateReader() },
            { GerberCommandType.FlashOperation, new FlashOperationReader() }
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