using PcbReader.Geometry;

namespace PcbReader.Layers.Svg.Entities;

public interface ISvgCursorDriver {
    public Point PointTo { get;  }
}