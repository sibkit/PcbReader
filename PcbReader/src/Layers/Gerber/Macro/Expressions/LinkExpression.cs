using PcbReader.Layers.Gerber.Entities.Macro;

namespace PcbReader.Layers.Gerber.Macro.Expressions;

public class LinkExpression: IExpression {
    public string Name { get; set; }
    public IExpression Value { get; set; }
}