using PcbReader.Layers.Common;
using PcbReader.Layers.Gerber.Entities.Apertures.Macro;

namespace PcbReader.Layers.Gerber.Entities;

public class GerberLayer
{
    public List<IPaintOperation> Operations { get; } = [];
    public Uom? Uom {get; set;} = null;
    public Dictionary<int,IAperture> Apertures { get; } = new();
    public Dictionary<string, MacroApertureTemplate> MacroApertureTemplates { get; } = new();
}

