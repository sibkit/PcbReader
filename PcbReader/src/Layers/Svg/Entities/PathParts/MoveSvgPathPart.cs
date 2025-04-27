using PcbReader.Geometry;

namespace PcbReader.Layers.Svg.Entities;

public class MoveSvgPathPart: ISvgPathPart, ISvgCursorDriver {
    public Point PointTo { get; set; }
}