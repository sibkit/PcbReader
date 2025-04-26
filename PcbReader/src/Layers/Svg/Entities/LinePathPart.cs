using PcbReader.Geometry;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Svg.Entities;

public class LinePathPart: IPathPart {
    public Point EndPoint { get; set; }
}