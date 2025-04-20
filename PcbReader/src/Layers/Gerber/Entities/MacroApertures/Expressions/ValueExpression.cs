namespace PcbReader.Layers.Gerber.Entities.MacroApertures.Expressions;

public class ValueExpression: IExpression {
    public ValueExpression(decimal value) {
        Value = value;
    }
    public decimal Value { get; set; }
}