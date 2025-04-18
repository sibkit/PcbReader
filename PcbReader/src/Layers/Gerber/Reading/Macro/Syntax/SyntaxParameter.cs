using PcbReader.Layers.Gerber.Reading.Macro.Tokenize;

namespace PcbReader.Layers.Gerber.Reading.Macro.Syntax;

public class SyntaxParameter(string name): ISyntaxOperand {
    public string Name { get; } = name;
    public IToken? Token { get; set; }
}