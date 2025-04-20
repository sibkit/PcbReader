namespace PcbReader.Layers.Gerber.Entities.MacroApertures.Expressions;

public enum OperationType {
    Add,
    Subtract,
    Multiply,
    Divide,
}

public class OperationExpression : IExpression {
    public OperationExpression(
        OperationType operationType,
        IExpression? leftExpression,
        IExpression? rightExpression) 
    {
        OperationType = operationType;
        LeftExpression = leftExpression;
        RightExpression = rightExpression;
    }

    public OperationExpression(OperationType operationType) 
    {
        OperationType = operationType;
    }
    
    public OperationType OperationType { get; set; }
    public IExpression? LeftExpression { get; set; }
    public IExpression? RightExpression { get; set; }
}