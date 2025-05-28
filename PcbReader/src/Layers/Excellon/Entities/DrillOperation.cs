﻿using PcbReader.Core;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Excellon.Entities;

public struct DrillOperation(Point point, int toolNumber) : IMachiningOperation {

    public Point StartPoint { get; set; } = point;
    public IMachiningOperation CloneWithShift(Point shift) {
        return new DrillOperation(this.StartPoint+shift, this.ToolNumber);
    }
    public int ToolNumber { get; set; } = toolNumber;
    
}