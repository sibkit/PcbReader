using System.Drawing;

namespace SibtronicPcbHandler;

public interface IBoardLayer {
    string Name { get; }
}

public interface IStackLayer : IBoardLayer {
    double Thickness { get; }
}

public class BoardBox {
    public Point InjectPoint { get; set; }
    public double Angle { get; set; }
}

public class Board {
    public List<BoardBox> ChildBoards { get; } = [];
    public List<IBoardLayer> Layers { get; } = [];
}

