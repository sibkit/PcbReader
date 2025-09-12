using PcbReader.Spv.Entities;

namespace SibtronicPcbHandler;

public class DrillingLayer: IBoardLayer {
    public SpvLayer Image { get; }
    public bool IsMetallization { get; }

    public DrillingLayer(SpvLayer image, bool isMetallization) {
        Image = image;
        IsMetallization = isMetallization;
    }
}