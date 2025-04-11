using PcbReader.Project;

namespace PcbReader.Layers.Excellon.Entities;

public interface IMachiningOperation
{
    public MachiningOperationType OperationType { get; }
    public Coordinate StartCoordinate { get; set; }

    public IMachiningOperation CloneWithShift(Coordinate shift);
}

public enum MachiningOperationType
{
    Drill,
    Mill
}