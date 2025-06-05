namespace PcbReader.Core.Entities;

public interface IGraphicElement {
    Bounds Bounds { get; }
    void UpdateBounds();
} 