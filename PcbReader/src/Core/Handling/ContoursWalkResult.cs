using PcbReader.Core.Entities.GraphicElements;

namespace PcbReader.Core.Handling;

public class ContoursWalkResult {
    public Contour OuterContour { get; init; }
    public List<Contour> InnerContours { get; init; }
}