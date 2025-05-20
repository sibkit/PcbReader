namespace PcbReader.Geometry.PathParts;

public class LinePathPart: IPathPart {
    public required Point PointTo { get; init; }
    public required Point PointFrom { get; init; }
    public IPathPartsOwner Owner { get; init; }
    
}