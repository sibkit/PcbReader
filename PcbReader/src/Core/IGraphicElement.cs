namespace PcbReader.Core;

public interface IGraphicElement {
    Bounds Bounds { get; }
    void UpdateBounds();
} 