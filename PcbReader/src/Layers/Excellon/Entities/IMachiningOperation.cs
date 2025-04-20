using PcbReader.Project;

namespace PcbReader.Layers.Excellon.Entities;

public interface IMachiningOperation
{
    public MachiningOperationType OperationType { get; }
    public Point StartPoint { get; set; }

    public IMachiningOperation CloneWithShift(Point shift);
}

public enum MachiningOperationType
{
    Drill,
    Mill
}