namespace PcbReader.Layers.Gerber.Entities.Apertures.Macro.Expressions;

public class ValueExpression: IExpression {
    public ValueExpression(decimal value) {
        Value = value;
    }
    public decimal Value { get; set; }
}