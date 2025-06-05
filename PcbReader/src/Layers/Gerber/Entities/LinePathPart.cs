using PcbReader.Core;
using PcbReader.Core.Entities;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Gerber.Entities;

public class LinePathPart: IPathPart {
    public LinePathPart(Point endPoint) {
        EndPoint = endPoint;
    }
    public Point EndPoint { get; set; }
}