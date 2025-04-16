namespace PcbReader.Layers.Gerber.Entities.StandardApertures;

public class CircleAperture : IAperture {
    public decimal Diameter { get; set; }
    public decimal? HoleDiameter { get; set; }
}