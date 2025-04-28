using PcbReader.Layers.Gerber.Reading.Macro.Tokenize;

namespace PcbReader.Layers.Gerber.Reading.Macro.Syntax;

public class SyntaxValue(double value): ISyntaxOperand {
    public double Value { get; } = value;
    public IToken? Token { get; set; }
}