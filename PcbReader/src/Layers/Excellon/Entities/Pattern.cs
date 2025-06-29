﻿using PcbReader.Layers.Common;
using PcbReader.Spv.Entities;

namespace PcbReader.Layers.Excellon.Entities;

public enum PatternState {
    Opened,
    Closed,
}

public class Pattern() {
    
    public Pattern(Point startPoint) : this() {
        StartPoint = startPoint;
    }
    public PatternState State { get; set; } = PatternState.Opened;
    public IList<IMachiningOperation> MachiningOperations { get; } = new List<IMachiningOperation>();
    
    public Point StartPoint { get; set; }
}