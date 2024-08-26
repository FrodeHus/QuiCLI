namespace QuiCLI.Command;

internal interface IBuilderState
{
    IEnumerable<object> Build();
}
