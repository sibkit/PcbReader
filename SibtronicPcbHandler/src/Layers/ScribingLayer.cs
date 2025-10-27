using PcbReader.Strx.Entities.GraphicElements.Curves;

namespace SibtronicPcbHandler.Layers;

public class ScribingLayer: IBoardLayer {
    public required string Name { get; set; }
    public List<Line> Lines { get; } = [];
}