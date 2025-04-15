using PcbReader.Layers.Gerber.Macro.Expressions;

namespace PcbReader.Layers.Gerber.Reading.Macro.Syntax;

public class SyntaxOperator(OperationType type): ISyntaxExpressionPart {

    public OperationType OperationType { get; } = type;
    
    public int GetPriority() {
        return OperationType switch {
            OperationType.Add or OperationType.Subtract => 1,
            OperationType.Multiply or OperationType.Divide => 2,
            _ => throw new Exception("Unknown operation type")
        };
    }
}