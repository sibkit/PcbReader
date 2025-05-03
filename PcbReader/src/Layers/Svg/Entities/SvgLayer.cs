using PcbReader.Geometry;

namespace PcbReader.Layers.Svg.Entities;

public class SvgLayer {
    public ViewBox? ViewBox { get; set; }
    public List<IVisible> Elements { get; } = [];
}