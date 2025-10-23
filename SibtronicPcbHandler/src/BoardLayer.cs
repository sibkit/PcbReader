namespace SibtronicPcbHandler;

public interface IBoardLayer { }
public class Board {
    public List<IBoardLayer> Layers { get; } = [];
}
