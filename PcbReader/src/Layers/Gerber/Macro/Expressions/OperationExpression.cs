namespace PcbReader.Layers.Gerber.Macro.Expressions;

public enum OperationType {
    Add,
    Subtract,
    Multiply,
    Divide,
}

public class OperationExpression {
    public OperationExpression(OperationType operationType) {
        OperationType = operationType;
    }
    
    public OperationType OperationType { get; set; }
    public List<IExpression> Expressions { get; } = [];
}