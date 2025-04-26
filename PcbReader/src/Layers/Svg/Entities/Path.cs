using PcbReader.Geometry;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Svg.Entities;

public class Path {
    public double StrokeWidth { get; set; }
    public Point StartPoint { get; set; }
    public List<IPathPart> Parts { get; } = [];
    public bool IsClosed { get; set; }
}