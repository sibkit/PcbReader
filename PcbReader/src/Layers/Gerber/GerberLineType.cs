namespace PcbReader.Layers.Gerber;

public enum GerberLineType {
    Comment,
    FormatSpecification,
    SetUom,
    DefineAperture,
    SetCoordinatesModeHandler,
    SetApertureHandler,
    DefineMacroApertureHandler,
    FlashOperation,
    MoveOperation,
    ArcSegmentOperation,
    LineSegmentOperation,
    SetLcMode,
    Ignored
}