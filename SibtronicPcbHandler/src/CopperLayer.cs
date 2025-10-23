using PcbReader.Strx.Entities;

namespace SibtronicPcbHandler;

public class CopperLayer: IBoardLayer {
    public StrxLayer Image { get; }
    public double Thickness { get; }

    public CopperLayer(StrxLayer image, double thickness) {
        Image = image;
        Thickness = thickness;
    }
}