using PcbReader.Project;

namespace PcbReader.Layers.Excellon.Entities;

public interface IMillPart
{
    public Coordinate EndCoordinate { get; }
    public MillPartType PartType { get; }
}

public enum MillPartType
{
    Arc,
    Linear
}