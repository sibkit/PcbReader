using PcbReader.Layers.Common;
using PcbReader.Spv.Entities;

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