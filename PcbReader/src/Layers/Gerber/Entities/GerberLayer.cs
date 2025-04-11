using PcbReader.Project;

namespace PcbReader.Layers.Gerber.Entities;

public class GerberLayer
{
    public List<IPaintOperation> Operations { get; } = [];
    public Uom? Uom {get; set;} = null;
    public Dictionary<int,IAperture> Apertures { get; } = new Dictionary<int, IAperture>();
    
}

