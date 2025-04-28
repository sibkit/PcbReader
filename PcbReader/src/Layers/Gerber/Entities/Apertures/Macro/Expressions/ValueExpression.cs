namespace PcbReader.Layers.Gerber.Entities.Apertures.Macro.Expressions;

public class ValueExpression: IExpression {
    public ValueExpression(double value) {
        Value = value;
    }
    public double Value { get; set; }
}