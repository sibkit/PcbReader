namespace PcbReader.Layers.Gerber.Macro.Expressions;

public class ParameterExpression(string name): IExpression {

    public string Name { get; set; } = name;
}