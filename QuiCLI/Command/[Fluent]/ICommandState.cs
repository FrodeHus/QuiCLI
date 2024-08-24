namespace QuiCLI.Command;

public interface ICommandState<TCommand> : IConfigureCommandInstance<TCommand> where TCommand : class
{
    ICommandState<TCommand> WithGroup(string groupName);
}