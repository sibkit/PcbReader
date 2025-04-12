namespace PcbReader.Layers;

public interface ICommandHandler<out T, in TC, in TP> 
    where T : Enum 
    where TC: Context {
    T[] GetNextLikelyTypes();
    bool Match(TC ctx);
    void WriteToProgram(TC ctx, TP program);
}