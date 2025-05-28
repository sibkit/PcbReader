namespace PcbReader.Core;

public interface IPathPart {
    Point PointFrom { get; }
    Point PointTo { get; }
    PathPartsOwner Owner { get; }

    Bounds Bounds { get; }
}