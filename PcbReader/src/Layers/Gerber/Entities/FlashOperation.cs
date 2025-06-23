using PcbReader.Layers.Common;
using PcbReader.Spv.Entities;

namespace PcbReader.Layers.Gerber.Entities;

public class FlashOperation: IPaintOperation {
    public Point Point{get;init;}
    public int ApertureCode{get;init;}
}