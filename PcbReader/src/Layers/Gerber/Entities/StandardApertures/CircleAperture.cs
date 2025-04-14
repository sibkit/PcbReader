namespace PcbReader.Layers.Gerber.Entities.Apertures;

public class CircleAperture : IAperture {
    public decimal Diameter { get; set; }
    public decimal? HoleDiameter { get; set; }
}