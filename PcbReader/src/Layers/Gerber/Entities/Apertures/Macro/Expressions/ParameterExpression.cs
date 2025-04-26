namespace PcbReader.Layers.Gerber.Entities.Apertures.Macro.Expressions;

public class ParameterExpression(string name): IExpression {

    public string Name { get; set; } = name;
}