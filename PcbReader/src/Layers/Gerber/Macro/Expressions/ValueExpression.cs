namespace PcbReader.Layers.Gerber.Macro.Expressions;

public class ValueExpression: IExpression {
    public ValueExpression(decimal value) {
        Value = value;
    }
    public decimal Value { get; set; }
}