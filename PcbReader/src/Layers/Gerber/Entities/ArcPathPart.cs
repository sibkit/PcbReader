using PcbReader.Core;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Gerber.Entities;

public class ArcPathPart: IPathPart {
    public Point EndPoint { get; set; }
    public double IOffset { get; set; }
    public double JOffset { get; set; }
    public RotationDirection RotationDirection { get; set; }
}