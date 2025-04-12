namespace PcbReader.Layers.Excellon.Handlers;

public class StartHeaderHandler : ICommandHandler<ExcellonCommandType, ExcellonContext, ExcellonLayer>
{
    public ExcellonCommandType[] GetNextLikelyTypes()
    {
        return [ExcellonCommandType.StartHeader];
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