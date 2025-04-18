using PcbReader.Layers.Gerber.Reading.Macro.Tokenize;

namespace PcbReader.Layers.Gerber.Reading.Macro.Syntax;

public class SyntaxGroup : ISyntaxOperand {
    public SyntaxExpression? Expression { get; set; }
    public IToken? Token { get; set; }
}