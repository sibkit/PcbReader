using PcbReader.Core.Entities;
using PcbReader.Core.Entities.GraphicElements;

namespace PcbReader.Core.Handling;

public class ExPoint {
    private List<(ICurve inCurve, ICurve outCurve)> Curves { get; } = [];

}