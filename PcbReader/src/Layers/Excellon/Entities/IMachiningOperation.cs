using PcbReader.Layers.Common;
using PcbReader.Strx.Entities;

namespace PcbReader.Layers.Excellon.Entities;

public interface IMachiningOperation
{
    public Point StartPoint { get; set; }

    public IMachiningOperation CloneWithShift(Point shift);
}

