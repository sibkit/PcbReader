using PcbReader.Project;

namespace PcbReader.Layers.Gerber.Entities;

public interface IPathPart {
    public Point EndPoint { get; }
}