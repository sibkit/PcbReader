namespace PcbReader.Spv.Entities.GraphicElements;

public interface ICurve: ICloneable {
    
    CurvesOwner Owner { get; set; }
    
    Point PointFrom { get; }
    Point PointTo { get; }

    void UpdateBounds();
    Bounds Bounds { get; }

    ICurve GetReversed();
    void Reverse();

}