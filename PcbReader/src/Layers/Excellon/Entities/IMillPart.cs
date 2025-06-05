using PcbReader.Core;
using PcbReader.Core.Entities;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Excellon.Entities;

public interface IMillPart
{
    public Point EndPoint { get; }
    public MillPartType PartType { get; }
}

public enum MillPartType
{
    Arc,
    Linear
}