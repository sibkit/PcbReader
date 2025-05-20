namespace PcbReader.Geometry;

public interface IPathPartsOwner: IVisible {
    List<IPathPart> Parts { get; }
    public Point StartPoint { get; set; }
}