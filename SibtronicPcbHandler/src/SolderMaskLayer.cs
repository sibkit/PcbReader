using PcbReader.Strx.Entities;

namespace SibtronicPcbHandler;

public enum SolderMaskColor {
    Any,
    White,
    Green,
    Black,
    MatteWhite,
    MatteBlack
}

public class SolderMaskLayer: IBoardLayer {
    public SolderMaskLayer(SolderMaskColor color, StrxLayer image) {
        Color = color;
        Image = image;
    }
    public SolderMaskColor Color { get; }
    public StrxLayer Image { get; }
}