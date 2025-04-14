using PcbReader.Layers.Gerber.Macro;

namespace PcbReader.Layers.Gerber.Entities.Macro.Expressions;

public class ValueExpression: IExpression {
    public ValueExpression(decimal value) {
        Value = value;
    }
    public decimal Value { get; set; }
}