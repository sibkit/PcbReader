using PcbReader.Core;
using PcbReader.Core.Entities;
using PcbReader.Layers.Common;
using PcbReader.Layers.Gerber.Entities.Apertures;

namespace PcbReader.Layers.Gerber.Entities;

public class PathPaintOperation: IPaintOperation {
    public PathPaintOperation(CircleAperture aperture, Point startPoint) {
        Aperture = aperture;
        StartPoint = startPoint;
    }
    public Point StartPoint { get; set; }
    public CircleAperture Aperture { get; set; }
    public List<IPathPart> Parts { get; } = [];
    public bool IsClosed { get; set; } = false;
}
