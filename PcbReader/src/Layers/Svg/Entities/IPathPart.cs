using PcbReader.Geometry;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Svg.Entities;

public interface IPathPart {
    Point EndPoint { get; set; }
}