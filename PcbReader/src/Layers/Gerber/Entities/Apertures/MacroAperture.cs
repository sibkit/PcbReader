namespace PcbReader.Layers.Gerber.Entities.Apertures;

public class MacroAperture: IAperture {
    public MacroAperture(string name) {
        Name = name;
    }
    public string Name {get;init;}
}