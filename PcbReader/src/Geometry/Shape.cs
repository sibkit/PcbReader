namespace PcbReader.Geometry;

public class Shape: IVisible {
    public required Contour OuterContour { get; init; }
    public List<Contour> InnerContours { get; } = [];
}