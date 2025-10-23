using PcbReader.Layers.Common;
using PcbReader.Strx.Entities;

namespace PcbReader.Layers.Gerber.Entities;

public interface IPathPart {
    public Point EndPoint { get; }
}