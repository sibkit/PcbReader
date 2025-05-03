namespace PcbReader.Geometry;

public class Shape: IVisible {
    public List<Contour> OuterContours { get; } = [];
    public List<Contour> InnerContours { get; } = [];
}