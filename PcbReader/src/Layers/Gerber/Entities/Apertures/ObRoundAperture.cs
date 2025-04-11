namespace PcbReader.Layers.Gerber.Entities.Apertures;

public class ObRoundAperture: IAperture {
    public decimal XSize { get; set; }
    public decimal YSize { get; set; }
    public decimal? HoleDiameter { get; set; }
}