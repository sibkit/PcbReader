namespace PcbReader.Spv.Entities;

public interface IGraphicElement {
    Bounds Bounds { get; }
    //void UpdateBounds();
    
    void Move(double dx, double dy);
    
} 