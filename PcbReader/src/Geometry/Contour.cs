namespace PcbReader.Geometry;

public class Contour: IVisible {
    public Point StartPoint { get; set; }
    public List<IPathPart> Parts { get; } = [];
}