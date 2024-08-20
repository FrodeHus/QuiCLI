namespace QuiCLI.Command;

public interface ICommandState<TCommand> : IConfigureCommandInstance<TCommand>, IConfigureCommandMethod<TCommand> where TCommand : class
{
    ICommandState<TCommand> WithGroup(string groupName);
}