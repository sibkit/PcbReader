using PcbReader.Layers.Gerber.Reading.Macro.Tokenize;

namespace PcbReader.Layers.Gerber.Reading.Macro.Syntax;

public class SyntaxValue(decimal value): ISyntaxOperand {
    public decimal Value { get; } = value;
    public IToken? Token { get; set; }
}