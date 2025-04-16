namespace PcbReader.Layers.Gerber.Entities.StandardApertures;

public class RectangleAperture: IAperture {
    public decimal XSize { get; set; }
    public decimal YSize { get; set; }
    public decimal? HoleDiameter { get; set; }
}