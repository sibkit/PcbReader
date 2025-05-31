namespace PcbReader.Core.GraphicElements;

public interface IPathPart {
    Point PointFrom { get; }
    Point PointTo { get; }

    void UpdateBounds();
    Bounds Bounds { get; }

    IPathPart GetReversed();
}