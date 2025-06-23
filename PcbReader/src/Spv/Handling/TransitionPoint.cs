using PcbReader.Spv.Entities.GraphicElements;

namespace PcbReader.Spv.Handling;



public class TransitionPoint {
    public ICurve InCurve { get; init; }
    public ICurve OutCurve { get; init; }
    public Contour Contour { get; init; }
}

