using PcbReader.Layers.Gerber.Reading.Macro.Tokenize;

namespace PcbReader.Layers.Gerber.Reading.Macro.Syntax;

public interface ISyntaxExpressionPart {
    public IToken Token { get; set; }
}