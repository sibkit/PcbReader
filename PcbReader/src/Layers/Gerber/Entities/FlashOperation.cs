using PcbReader.Layers.Common;

namespace PcbReader.Layers.Gerber.Entities;

public class FlashOperation: IPaintOperation {
    public Point Point{get;set;}
    public int ApertureCode{get;set;}
}