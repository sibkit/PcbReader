namespace PcbReader.Layers.Gerber;

public enum GerberCommandType {
    Comment,
    FormatSpecification,
    SetUom,
    DefineAperture,
    SetCoordinates,
    SetAperture,
    DefineMacroAperture,
    FlashOperation,
    MoveOperation,
    ArcSegmentOperation,
    LineSegmentOperation,
    SetLcMode,
    Ignored
}