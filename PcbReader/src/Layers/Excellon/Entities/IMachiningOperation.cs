using PcbReader.Layers.Common;
using PcbReader.Spv.Entities;

namespace PcbReader.Layers.Excellon.Entities;

public interface IMachiningOperation
{
    public Point StartPoint { get; set; }

    public IMachiningOperation CloneWithShift(Point shift);
}

