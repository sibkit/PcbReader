namespace PcbReader.Layers.Gerber.Entities.Apertures;

public class CircleAperture : IAperture {
    public double Diameter { get; set; }
    public double? HoleDiameter { get; set; }
}