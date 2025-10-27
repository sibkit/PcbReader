using PcbReader.Strx.Entities;

namespace SibtronicPcbHandler.Layers;

public enum SolderMaskColor {
    Any,
    White,
    Green,
    Black,
    MatteWhite,
    MatteBlack
}

public class SolderMaskLayer: IBoardLayer {
    public required string Name { get; set; }
    public SolderMaskLayer(SolderMaskColor color, StrxLayer image) {
        Color = color;
        Image = image;
    }
    public SolderMaskColor Color { get; }
    public StrxLayer Image { get; }
}