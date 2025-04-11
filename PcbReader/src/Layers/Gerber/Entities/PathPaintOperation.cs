using PcbReader.Layers.Gerber.Entities.Apertures;
using PcbReader.Project;

namespace PcbReader.Layers.Gerber.Entities;

public class PathPaintOperation {
    
    public PathPaintOperation(CircleAperture aperture, Coordinate startCoordinate) {
        Aperture = aperture;
        StartCoordinate = startCoordinate;
    }
    public Coordinate StartCoordinate { get; set; }
    public CircleAperture Aperture { get; set; }
    public List<IPathPart> Parts { get; } = [];
}
