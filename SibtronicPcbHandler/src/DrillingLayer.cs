using PcbReader.Strx.Entities;

namespace SibtronicPcbHandler;

public class DrillingLayer: IBoardLayer {
    public StrxLayer Image { get; }
    public bool IsMetallization { get; }

    public DrillingLayer(StrxLayer image, bool isMetallization) {
        Image = image;
        IsMetallization = isMetallization;
    }
}