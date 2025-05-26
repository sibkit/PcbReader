namespace PcbReader.Geometry;

public interface IPathPart {
    Point PointFrom { get; }
    Point PointTo { get; }
    IPathPartsOwner Owner { get; }

    Bounds Bounds { get; }
}