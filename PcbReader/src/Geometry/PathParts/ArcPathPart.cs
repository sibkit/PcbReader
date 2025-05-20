namespace PcbReader.Geometry.PathParts;

public class ArcPathPart : IPathPart {
    public required Point PointTo { get; init; }
    public required Point PointFrom { get; init; }
    public required IPathPartsOwner Owner { get; init; }
    public required double Radius { get; init; }
    public required bool IsLargeArc { get; init; } = false;
    public required RotationDirection RotationDirection { get; set; } = 0;
    

}