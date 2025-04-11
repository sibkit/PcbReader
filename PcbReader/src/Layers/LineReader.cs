using System.Text;

namespace PcbReader.Layers;

public abstract class LineReader<T, TC, TP> 
    where T : Enum 
    where TC: Context, new()
    where TP: new() {
    private readonly Dictionary<T, ILineHandler<T,TC,TP>> _handlers;
    private T[] _nextLikelyTypes;

    protected LineReader(Dictionary<T, ILineHandler<T,TC,TP>> handlers, T[] nextLikelyTypes) {
        _nextLikelyTypes = nextLikelyTypes;
        _handlers = handlers;
    }

    protected abstract IEnumerable<string> ExcludeLines(string text);
    
    public  (TP, TC) ReadProgram(FileInfo file) {
        var program = new TP();
        using var streamReader = new StreamReader(
            file.Open(FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.UTF8);
        

        List<string> lines = [];
        lines.AddRange(ExcludeLines(streamReader.ReadToEnd()));


        var ctx = new TC();
        ctx.Init(lines);
        foreach (var unused in lines) {
            HandleRow(ctx, program);
            if(!ctx.ContinueHandle) break;
            ctx.CurIndex++;
        }
        return (program, ctx);
    }

    
    
    private bool MatchAndHandle(ILineHandler<T, TC, TP> handler, TC ctx, TP program) {
        if (!handler.Match(ctx)) return false;
        handler.WriteToProgram(ctx, program);
        _nextLikelyTypes = handler.GetNextLikelyTypes();
        return true;
    }

    private uint _counter;
    
    private void HandleRow(TC ctx, TP program) {
        if (ctx.CurLine == "") return;
        foreach (var lt in _nextLikelyTypes) {
            if (!_handlers.TryGetValue(lt, out var handler)) {
                throw new Exception("No handler registered for type: \"" + lt + "\"");
            }
            if (MatchAndHandle(handler, ctx, program)) {
                return;
            }
        }

        if (_handlers.Any(entry => MatchAndHandle(entry.Value, ctx, program))) {
            return;
        }

        _counter++;
        ctx.WriteError(_counter.ToString("D4") + " Не найден обработчик для строки: \"" + ctx.CurLine + "\"");
    }
}