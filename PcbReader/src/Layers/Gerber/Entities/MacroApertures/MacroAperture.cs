using PcbReader.Layers.Gerber.Entities.Macro;
using PcbReader.Layers.Gerber.Entities.Macro.Expressions;
using PcbReader.Layers.Gerber.Macro;
using PcbReader.Layers.Gerber.Macro.Expressions;

namespace PcbReader.Layers.Gerber.Entities.MacroApertures;

public class MacroAperture: IAperture {
    public MacroAperture(string name) {
        Name = name;
    }

    public string Name {get;}
    public List<IPrimitive> Primitives {get;} = [];
    public List<IExpression> Parameters {get;} = [];
    public List<LinkExpression> Expressions {get;} = [];
    public List<decimal> Values {get;} = [];
}