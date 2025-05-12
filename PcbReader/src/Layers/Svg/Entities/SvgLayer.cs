using PcbReader.Geometry;

namespace PcbReader.Layers.Svg.Entities;

public class SvgLayer {
    public Bounds? ViewBox { get; set; }
    public List<IVisible> Elements { get; } = [];
}