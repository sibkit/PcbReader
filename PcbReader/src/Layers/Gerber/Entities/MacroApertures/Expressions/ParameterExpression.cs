namespace PcbReader.Layers.Gerber.Entities.MacroApertures.Expressions;

public class ParameterExpression(string name): IExpression {

    public string Name { get; set; } = name;
}