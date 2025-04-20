using PcbReader.Project;

namespace PcbReader.Layers.Gerber.Entities;

public class ArcPathPart: IPathPart {
    public Point EndPoint { get; set; }
    public decimal IOffset { get; set; }
    public decimal JOffset { get; set; }
}