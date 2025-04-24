using PcbReader.Layers.Common;
using PcbReader.Layers.Gerber.Entities.StandardApertures;

namespace PcbReader.Layers.Gerber.Entities;

public class PathPaintOperation {
    public PathPaintOperation(CircleAperture aperture, Point startPoint) {
        Aperture = aperture;
        StartPoint = startPoint;
    }
    public Point StartPoint { get; set; }
    public CircleAperture Aperture { get; set; }
    public List<IPathPart> Parts { get; } = [];
}
