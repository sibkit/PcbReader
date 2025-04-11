using PcbReader.Project;

namespace PcbReader.Layers.Gerber.Entities;

public class FlashOperation: IPaintOperation {
    public Coordinate Coordinate{get;set;}
    public int ApertureCode{get;set;}
}