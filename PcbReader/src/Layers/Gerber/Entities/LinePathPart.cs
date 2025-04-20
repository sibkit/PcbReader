using PcbReader.Project;

namespace PcbReader.Layers.Gerber.Entities;

public class LinePathPart: IPathPart {
    public LinePathPart(Point endPoint) {
        EndPoint = endPoint;
    }
    public Point EndPoint { get; init; }
}