using PcbReader.Layers.Common;

namespace PcbReader.Layers.Svg.Entities;

public class Path {
    public Point StartPoint { get; set; }
    public List<IPathPart> Parts { get; } = [];
}