namespace PcbReader.Core.GraphicElements;

public interface ICurve {
    Point PointFrom { get; }
    Point PointTo { get; }

    void UpdateBounds();
    Bounds Bounds { get; }

    ICurve GetReversed();
}