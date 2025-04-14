namespace PcbReader.Layers.Gerber;

public enum GerberCommandType {
    Comment,
    FormatSpecification,
    SetUom,
    DefineAperture,
    SetCoordinates,
    SetAperture,
    DefineApertureMacro,
    FlashOperation,
    MoveOperation,
    ArcSegmentOperation,
    LineSegmentOperation,
    SetLcMode,
    Ignored
}