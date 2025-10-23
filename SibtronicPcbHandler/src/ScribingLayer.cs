using PcbReader.Strx.Entities.GraphicElements.Curves;

namespace SibtronicPcbHandler;

public class ScribingLayer: IBoardLayer {
    public List<Line> Lines { get; } = [];
}