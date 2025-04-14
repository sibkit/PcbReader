// See https://aka.ms/new-console-template for more information

using PcbReader;
using PcbReader.Layers.Gerber.Macro;

//Reader.TestReader();


var lexer = new Tokenizer();
var tokens = lexer.Tokenize("$4x0.25x(5.16-$3)");
Console.WriteLine("ok");