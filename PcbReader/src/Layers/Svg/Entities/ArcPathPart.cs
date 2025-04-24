using PcbReader.Layers.Common;

namespace PcbReader.Layers.Svg.Entities;

public class ArcPathPart : IPathPart {
    public Point EndPoint { get; set; }
    public decimal Radius { get; set; }
    public bool IsLargeArc { get; set; } = false;
    public RotationDirection RotationDirection { get; set; } = 0;
}