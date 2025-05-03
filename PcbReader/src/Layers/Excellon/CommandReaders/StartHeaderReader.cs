using PcbReader.Layers.Common.Reading;
using PcbReader.Layers.Excellon.Entities;

namespace PcbReader.Layers.Excellon.CommandReaders;

public class StartHeaderReader : ICommandReader<ExcellonCommandType, ExcellonReadingContext, ExcellonLayer>
{
    public ExcellonCommandType[] GetNextLikelyTypes()
    {
        return [ExcellonCommandType.StartHeader];
    }

    public bool Match(ExcellonReadingContext ctx)
    {
        return ctx.CurLine.Trim().ToUpper().Equals("M48");
    }

    public void WriteToProgram(ExcellonReadingContext ctx, ExcellonLayer layer)
    {
        //Do nothing;
    }
}