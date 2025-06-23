namespace PcbReader.Spv.Entities;

public interface IGraphicElement {
    Bounds Bounds { get; }
    void UpdateBounds();
} 