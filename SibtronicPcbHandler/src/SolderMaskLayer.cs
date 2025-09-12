using PcbReader.Spv.Entities;

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
    public SolderMaskLayer(SolderMaskColor color, SpvLayer image) {
        Color = color;
        Image = image;
    }
    public SolderMaskColor Color { get; }
    public SpvLayer Image { get; }
}