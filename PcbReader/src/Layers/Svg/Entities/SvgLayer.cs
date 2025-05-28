using PcbReader.Core;

namespace PcbReader.Layers.Svg.Entities;

public class SvgLayer {
    public Bounds? ViewBox { get; set; }
    public List<IGraphicElement> Elements { get; } = [];
}