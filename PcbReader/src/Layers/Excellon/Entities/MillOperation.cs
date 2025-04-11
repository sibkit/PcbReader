using PcbReader.Project;

namespace PcbReader.Layers.Excellon.Entities;

public readonly struct ArcMillPart(Coordinate endCoordinate, float radius) {
    public Coordinate EndCoordinate { get; } = endCoordinate;
    public MillPartType PartType { get; } = MillPartType.Arc;
    public float Radius { get; } = radius;
}

public readonly struct LinearMillPart(Coordinate endCoordinate) : IMillPart {
    public Coordinate EndCoordinate { get; } = endCoordinate;
    public MillPartType PartType { get; } = MillPartType.Linear;
    
}

public class MillOperation: IMachiningOperation {

    public MachiningOperationType OperationType { get; } = MachiningOperationType.Mill;
    public Coordinate StartCoordinate { get; set; }

    public List<IMillPart> MillParts { get; } = [];
    
    public IMachiningOperation CloneWithShift(Coordinate shift) {
        var result = new MillOperation {
            StartCoordinate = StartCoordinate + shift
        };
        return result;
    }
}