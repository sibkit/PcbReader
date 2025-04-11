using PcbReader.Layers.Excellon.Entities;
using PcbReader.Project;

namespace PcbReader.Layers.Excellon;

public class ExcellonLayer
{
    public List<IMachiningOperation> Operations { get; } = [];
    public Uom? Uom {get; set;} = null;
    public Dictionary<int,decimal> ToolsMap { get; } = new Dictionary<int, decimal>();
}

