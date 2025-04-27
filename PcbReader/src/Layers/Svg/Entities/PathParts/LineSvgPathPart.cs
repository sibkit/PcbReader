using PcbReader.Geometry;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Svg.Entities;

public class LineSvgPathPart: ISvgPathPart, ISvgCursorDriver {
    public Point PointTo { get; set; }
}