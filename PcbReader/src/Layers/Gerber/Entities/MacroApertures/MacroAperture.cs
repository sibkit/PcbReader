using PcbReader.Layers.Gerber.Macro;
using PcbReader.Layers.Gerber.Macro.Expressions;

namespace PcbReader.Layers.Gerber.Entities.MacroApertures;

public class MacroAperture: IAperture {
    public MacroAperture(string templateName) {
        TemplateName = templateName;
    }

    public string TemplateName {get;}
    public List<decimal> ParameterValues {get;} = [];
}