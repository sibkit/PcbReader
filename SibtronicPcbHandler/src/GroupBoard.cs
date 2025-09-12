using PcbReader.Spv.Entities;

namespace SibtronicPcbHandler;

public class GroupBoard {
    public Board Board { get; set; }
    public int Rows { get; set; }
    public int Columns { get; set; }
    public double HorizontalMargins { get; set; }
    public double VerticalMargins { get; set; }
    public SpvLayer? MillingLayer { get; set; }
    public SpvLayer? ScrabbingLayer { get; set; }
}