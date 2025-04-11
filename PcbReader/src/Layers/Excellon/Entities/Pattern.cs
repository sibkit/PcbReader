using PcbReader.Project;

namespace PcbReader.Layers.Excellon.Entities;

public enum PatternState {
    Opened,
    Closed,
}

public class Pattern() {
    
    public Pattern(Coordinate startPoint) : this() {
        StartPoint = startPoint;
    }
    public PatternState State { get; set; } = PatternState.Opened;
    public IList<IMachiningOperation> MachiningOperations { get; } = new List<IMachiningOperation>();
    
    public Coordinate StartPoint { get; set; }
}