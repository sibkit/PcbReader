namespace PcbReader.Layers.Gerber.Macro.Expressions;

public class OperationExpression {
    public OperationExpression(IExpression leftExpression, IExpression rightExpression, OperationType operationType) {
        LeftExpression = leftExpression;
        RightExpression = rightExpression;
        OperationType = operationType;
    }
    
    public OperationType OperationType { get; set; }
    public IExpression LeftExpression { get; set; }
    public IExpression RightExpression { get; set; }
}