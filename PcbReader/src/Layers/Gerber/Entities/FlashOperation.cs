using PcbReader.Core;
using PcbReader.Layers.Common;

namespace PcbReader.Layers.Gerber.Entities;

public class FlashOperation: IPaintOperation {
    public Point Point{get;init;}
    public int ApertureCode{get;init;}
}