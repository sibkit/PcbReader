using PcbReader.Core.Intersections;

namespace PcbReader.Core.GraphicElements;

public interface ICurve: ICloneable {
    Point PointFrom { get; }
    Point PointTo { get; }

    void UpdateBounds();
    Bounds Bounds { get; }

    ICurve GetReversed();
}