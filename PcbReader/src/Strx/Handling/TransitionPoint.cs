using PcbReader.Strx.Entities.GraphicElements;

namespace PcbReader.Strx.Handling;



public class TransitionPoint {
    public ICurve InCurve { get; init; }
    public ICurve OutCurve { get; init; }
    public Contour Contour { get; init; }
}

