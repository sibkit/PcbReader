using PcbReader.Layers.Gerber.Reading.Macro.Syntax;
using PcbReader.Layers.Gerber.Reading.Macro.Tokenize;

namespace PcbReader.Layers.Gerber.Reading.Macro;


public class SyntaxExpressionBuilder {

    private (SyntaxExpression, int) BuildExpression(int index) {
        var isOdd = true;
        var shift = 0;
        var result = new SyntaxExpression();
        for (var i = index; i < _tokens.Count; i++) {
            switch (_tokens[i]) {
                case ParensOpenToken:

                    if (!isOdd)
                        throw new ApplicationException("Unexpected operand, must be operator");

                    var group = new SyntaxGroup();
                    var buildResult = BuildExpression(i + 1);
                    group.Expression = buildResult.Item1;
                    i += buildResult.Item2 + 2;
                    shift += buildResult.Item2 + 2;
                    if (_tokens[i - 1] is not ParensCloseToken) {
                        throw new ApplicationException("Unexpected end of expression");
                    }

                    result.Parts.Add(group);
                    break;
                case ParensCloseToken:
                    if(index==0)
                        throw new ApplicationException("Unexpected parent close");
                    return (result, shift);
                case OperationToken ot:
                    if (isOdd)
                        throw new ApplicationException("Unexpected operator, must be operator");
                    var sop = new SyntaxOperator(ot.Type);
                    result.Parts.Add(sop);
                    break;
                case ValueToken vt:
                    if (!isOdd)
                        throw new ApplicationException("Unexpected operand, must be operator");
                    result.Parts.Add(new SyntaxValue(vt.Value));
                    break;
                case ParameterToken pt:
                    if (!isOdd)
                        throw new ApplicationException("Unexpected operand, must be operator");
                    result.Parts.Add(new SyntaxParameter(pt.Name));
                    break;
                default:
                    throw new Exception("Unexpected operator");

            }

            shift++;
            isOdd = !isOdd;
        }

        if(index != 0)
            throw new ApplicationException("Unexpected end of expression");
        return (result, shift);
    }

    private IList<IToken> _tokens;
    
    public SyntaxExpression Build(string expression) {
        _tokens = new Tokenizer().Tokenize(expression);
        var result = BuildExpression(0);
        return result.Item1;
    }

}