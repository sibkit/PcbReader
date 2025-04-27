using PcbReader.Geometry;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Svg.Entities;

public class ArcSvgPathPart : ISvgPathPart, ISvgCursorDriver {
    public Point PointTo { get; set; }
    public double Radius { get; set; }
    public bool IsLargeArc { get; set; } = false;
    public RotationDirection RotationDirection { get; set; } = 0;
}