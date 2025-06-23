using PcbReader.Spv.Entities;

namespace PcbReader.Layers.Svg.Entities;

public class SvgLayer {
    public Bounds? ViewBox { get; set; }
    public List<IGraphicElement> Elements { get; } = [];
}