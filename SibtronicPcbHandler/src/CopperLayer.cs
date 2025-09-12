using PcbReader.Spv.Entities;

namespace SibtronicPcbHandler;

public class CopperLayer: IBoardLayer {
    public SpvLayer Image { get; }
    public double Thickness { get; }

    public CopperLayer(SpvLayer image, double thickness) {
        Image = image;
        Thickness = thickness;
    }
}