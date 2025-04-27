using PcbReader.Layers.Gerber.Entities.Apertures.Macro.Expressions;

namespace PcbReader.Layers.Gerber.Entities.Apertures.Macro;

public class MacroApertureTemplate {
    public MacroApertureTemplate(string templateName) {
        Name = templateName;
    }
    
    public string Name {get;}
    public List<IPrimitive> Primitives {get;} = [];
    public List<IExpression> Parameters {get;} = [];
    public List<ParameterExpression> Expressions {get;} = [];
}