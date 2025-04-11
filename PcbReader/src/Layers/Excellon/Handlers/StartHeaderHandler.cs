namespace PcbReader.Layers.Excellon.Handlers;

public class StartHeaderHandler : ILineHandler<ExcellonLineType, ExcellonContext, ExcellonLayer>
{
    public ExcellonLineType[] GetNextLikelyTypes()
    {
        return [ExcellonLineType.StartHeader];
    }

    public bool Match(ExcellonContext ctx)
    {
        return ctx.CurLine.Trim().ToUpper().Equals("M48");
    }

    public void WriteToProgram(ExcellonContext ctx, ExcellonLayer layer)
    {
        //Do nothing;
    }
}