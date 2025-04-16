namespace PcbReader.Layers.Gerber.Entities.StandardApertures;

public class PolygonAperture: IAperture {
    public decimal OuterDiameter { get; set; }
    public int VerticesCount { get; set; }
    public decimal? Rotation { get; set; }
    public decimal? HoleDiameter { get; set; }
}