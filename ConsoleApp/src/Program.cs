// See https://aka.ms/new-console-template for more information

using PcbReader;
using PcbReader.Layers.Gerber.Macro;
using PcbReader.Layers.Gerber.Reading.Macro;
using PcbReader.Layers.Gerber.Reading.Macro.Tokenize;

//Reader.TestReader();


var lexer = new Tokenizer();
var text = "$4+0.25x(5.16-$3)";
var tokens = lexer.Tokenize(text);



var builder = new SyntaxExpressionBuilder();
var node = builder.Build(text);


Console.WriteLine("ok");