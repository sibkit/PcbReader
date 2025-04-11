using PcbReader.Project;

namespace PcbReader.Layers.Gerber.Entities;

public class LinePathPart: IPathPart {
    public LinePathPart(Coordinate endCoordinate) {
        EndCoordinate = endCoordinate;
    }
    public Coordinate EndCoordinate { get; init; }
}