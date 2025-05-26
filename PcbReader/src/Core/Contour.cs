using System.Collections.ObjectModel;
using System.ComponentModel;


namespace PcbReader.Geometry;

public class Contour: IPathPartsOwner {
    public Point StartPoint { get; set; }
    public List<IPathPart> Parts { get; } = [];
    
}