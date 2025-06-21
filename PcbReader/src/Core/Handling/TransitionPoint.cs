using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements;

namespace PcbReader.Core.Handling;



public class TransitionPoint {
    public ICurve InCurve { get; init; }
    public ICurve OutCurve { get; init; }
    public Contour Contour { get; init; }
}

