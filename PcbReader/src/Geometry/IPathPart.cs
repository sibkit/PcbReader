namespace PcbReader.Geometry;

public interface IPathPart {
    Point PointTo { get; }
    IPathPartsOwner Owner { get; }

    Point PointFrom { get; }
}