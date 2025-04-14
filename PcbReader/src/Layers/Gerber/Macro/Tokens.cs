using PcbReader.Layers.Gerber.Entities.Macro.Expressions;
using PcbReader.Layers.Gerber.Macro.Expressions;

namespace PcbReader.Layers.Gerber.Macro;


public interface IToken;

public struct ParensOpenToken: IToken;
public struct ParensCloseToken: IToken;

public struct AssignToken: IToken;

public readonly struct ValueToken(decimal value) : IToken {
    public decimal Value { get; init; } = value;
}

public readonly struct ParameterToken(string name) : IToken {
    public string Name { get; init; } = name;
}

public readonly struct OperationToken(OperationType type) : IToken {
    public OperationType Type { get; init; } = type;
}

