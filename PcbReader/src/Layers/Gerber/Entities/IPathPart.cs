using PcbReader.Core;
using PcbReader.Core.Entities;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Gerber.Entities;

public interface IPathPart {
    public Point EndPoint { get; }
}