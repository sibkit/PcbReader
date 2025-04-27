using PcbReader.Geometry;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Svg.Entities;

public class SvgLayer {
    public Rect? ViewBox { get; set; }
    public List<SvgPath> Paths { get; } = [];
}