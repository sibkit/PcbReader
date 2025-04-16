using System.Globalization;
using PcbReader.Layers.Gerber.Macro;
using PcbReader.Layers.Gerber.Macro.Expressions;
using PcbReader.Layers.Gerber.Reading.Macro;
using PcbReader.Layers.Gerber.Reading.Macro.Tokenize;

namespace ConsoleApp;

public static class ExpressionsTest {
    public static void Test() {
        Console.WriteLine((4*3).ToString(CultureInfo.InvariantCulture));

        var lexer = new Tokenizer();
//const string text = "1 + 7 x 3 + (2 + 1) x 4 x 6 + 4 x 2";
        const string text = "(2+2*2)/3";
        var tokens = lexer.Tokenize(text);

        var builder = new ExpressionBuilder();
        var node = builder.Build(text);

        Console.WriteLine(text + " = " + CalculateExpression(node));
        Console.WriteLine(PrintExpression(node));
        Console.WriteLine("ok");
    }
    
    static string PrintOperation(OperationType operation) {
        return operation switch {
            OperationType.Add => "+",
            OperationType.Subtract => "-",
            OperationType.Multiply => "*",
            OperationType.Divide => "/",
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };
    }

    static string PrintExpression(IExpression? expression) {
        return expression switch {
            OperationExpression op => "(" +PrintExpression(op.LeftExpression) + PrintOperation(op.OperationType) +
                                      PrintExpression(op.RightExpression) + ")",
            ValueExpression ve => ve.Value.ToString(CultureInfo.InvariantCulture),
            ParameterExpression pe => pe.Name,
            null => "null",
            _ => throw new ArgumentOutOfRangeException(nameof(expression), expression, null)
        };
    }



    static decimal CalculateExpression(IExpression? expression) {
        return expression switch {
            OperationExpression op => op.OperationType switch {
                OperationType.Add => CalculateExpression(op.LeftExpression) + CalculateExpression(op.RightExpression),
                OperationType.Subtract => CalculateExpression(op.LeftExpression) - CalculateExpression(op.RightExpression),
                OperationType.Multiply => CalculateExpression(op.LeftExpression) * CalculateExpression(op.RightExpression),
                OperationType.Divide => CalculateExpression(op.LeftExpression) / CalculateExpression(op.RightExpression),
                _ => throw new ArgumentOutOfRangeException(nameof(expression), expression, null)
            },
            ValueExpression ve => ve.Value,
            ParameterExpression pe => throw new Exception("Not implemented"),
            _ => throw new ArgumentOutOfRangeException(nameof(expression), expression, null)
        };
        ;
    }
    
}