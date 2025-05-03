namespace PcbReader.Geometry.PathParts;

public class ArcPathPart : IPathPart {
    public Point PointTo { get; set; }
    public double Radius { get; set; }
    public bool IsLargeArc { get; set; } = false;
    public RotationDirection RotationDirection { get; set; } = 0;
}