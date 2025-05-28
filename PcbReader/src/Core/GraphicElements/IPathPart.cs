namespace PcbReader.Core.GraphicElements;

public interface IPathPart {
    Point PointFrom { get; }
    Point PointTo { get; }

    Bounds Bounds { get; }

    IPathPart GetReversed();
}