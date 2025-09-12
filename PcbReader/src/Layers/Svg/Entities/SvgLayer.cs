using PcbReader.Spv.Entities;

namespace PcbReader.Layers.Svg.Entities;

public class SvgLayer {
    public Bounds? ViewBox { get; set; }
    public double? Width { get; set; }
    public double? Height { get; set; }
    public List<IGraphicElement> Elements { get; } = [];
}