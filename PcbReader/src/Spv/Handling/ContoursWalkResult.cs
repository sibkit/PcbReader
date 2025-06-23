using PcbReader.Spv.Entities.GraphicElements;

namespace PcbReader.Spv.Handling;

public class ContoursWalkResult {
    public Contour OuterContour { get; init; }
    public List<Contour> InnerContours { get; init; }
}