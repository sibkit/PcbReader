using System.Diagnostics;
using PcbReader.Layers.Gerber.Macro;
using PcbReader.Layers.Gerber.Macro.Expressions;
using PcbReader.Layers.Gerber.Reading.Macro.Syntax;

namespace PcbReader.Layers.Gerber.Reading.Macro;

public class ExpressionBuilder {


    
    private IExpression BuildExpression(ISyntaxOperand operand) {
        return operand switch {
            SyntaxGroup group => BuildExpression(group.Expression ??
                                                 throw new ApplicationException("Invalid Group operand")),
            SyntaxParameter parameter => new ParameterExpression(parameter.Name),
            SyntaxValue value => new ValueExpression(value.Value),
            _ => throw new Exception("Unknown SyntaxOperand type")
        };
    }
    
    private IExpression BuildExpression(SyntaxExpression se) {
        if (se.Parts.Count == 1) {
            return BuildExpression((se.Parts[0] as ISyntaxOperand)!);
        }
        
        var stack = new Stack<OperationExpression>();
        
        for (var i = 1; i<se.Parts.Count; i+=2) {
            var curOperator = se.Parts[i] as SyntaxOperator;
            
            var leftOperator = i > 2 ? se.Parts[i - 2] as SyntaxOperator: null;
            var rightOperator = i + 2 < se.Parts.Count-1 ? se.Parts[i + 2] as SyntaxOperator: null;

            var curExpr = new OperationExpression(curOperator!.OperationType);
            
            if (leftOperator == null || leftOperator.GetPriority() < curOperator!.GetPriority()) {
                curExpr.LeftExpression = BuildExpression((se.Parts[i-1] as ISyntaxOperand)!);
            } else {
                curExpr.LeftExpression = stack.Pop()!;
            }

            if (rightOperator == null || rightOperator.GetPriority() <= curOperator!.GetPriority()) {
                curExpr.RightExpression = BuildExpression((se.Parts[i+1] as ISyntaxOperand)!);
            } else {
                curExpr.RightExpression = null;
            }
            
            if (leftOperator == null || leftOperator.GetPriority() >= curOperator!.GetPriority()) {
                while (stack.Count != 0) {
                    var prevExpr = stack.Pop();
                    if (prevExpr.RightExpression == null) {
                        prevExpr.RightExpression = curExpr;
                        curExpr = prevExpr;
                    }
                }
            }
            
            stack.Push(curExpr);
        }

        var resultExpr = stack.Pop();
        
        while (stack.Count != 0) {
            var prevExpr = stack.Pop();
            if (prevExpr.RightExpression == null) {
                prevExpr.RightExpression = resultExpr;
                resultExpr = prevExpr;
            } else {
                throw new Exception("Unexpected Expression");
            }
        }
        

        return resultExpr;
    }
    
    public IExpression Build(string text) {
        var seb = new SyntaxExpressionBuilder();
        return BuildExpression(seb.Build(text));
    }
}