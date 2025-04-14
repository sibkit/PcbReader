using PcbReader.Layers.Gerber.Entities.Apertures;
using PcbReader.Layers.Gerber.Entities.Macro;
using PcbReader.Layers.Gerber.Entities.MacroApertures;
using PcbReader.Project;

namespace PcbReader.Layers.Gerber.Entities;

public class GerberLayer
{
    public List<IPaintOperation> Operations { get; } = [];
    public Uom? Uom {get; set;} = null;
    public Dictionary<int,IAperture> Apertures { get; } = new();
    public Dictionary<string, MacroAperture> MacroApertures { get; } = new();
}

