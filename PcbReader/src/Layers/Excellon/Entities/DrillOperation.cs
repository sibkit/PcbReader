using PcbReader.Project;

namespace PcbReader.Layers.Excellon.Entities;

public struct DrillOperation(Coordinate coordinate, int toolNumber) : IMachiningOperation {
    public MachiningOperationType OperationType => MachiningOperationType.Drill;

    public Coordinate StartCoordinate { get; set; } = coordinate;
    public IMachiningOperation CloneWithShift(Coordinate shift) {
        return new DrillOperation(this.StartCoordinate+shift, this.ToolNumber);
    }
    public int ToolNumber { get; set; } = toolNumber;
    
}