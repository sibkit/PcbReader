namespace PcbReader.Core.Entities.GraphicElements;

public interface ICurve: ICloneable {
    Point PointFrom { get; }
    Point PointTo { get; }

    void UpdateBounds();
    Bounds Bounds { get; }

    ICurve GetReversed();
}