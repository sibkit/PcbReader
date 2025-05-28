using PcbReader.Core;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Excellon.Entities;

public interface IMachiningOperation
{
    public Point StartPoint { get; set; }

    public IMachiningOperation CloneWithShift(Point shift);
}

