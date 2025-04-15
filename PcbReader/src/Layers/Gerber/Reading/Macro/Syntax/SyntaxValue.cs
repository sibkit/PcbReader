namespace PcbReader.Layers.Gerber.Reading.Macro.Syntax;

public class SyntaxValue(decimal value): ISyntaxOperand {
    public decimal Value { get; } = value;
}