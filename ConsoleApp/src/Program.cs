// See https://aka.ms/new-console-template for more information

using PcbReader;

Reader.TestReader();

var e = ExcludeCommands();
foreach(var c in e)
    Console.WriteLine(c);
return;


IEnumerable<string> ExcludeCommands() {
    yield return "a";
    yield return "b";
    yield return "c";
    // return reader.ReadToEnd().Split(["\n","\r","%","*"],StringSplitOptions.RemoveEmptyEntries).Where(str => str!="");
}