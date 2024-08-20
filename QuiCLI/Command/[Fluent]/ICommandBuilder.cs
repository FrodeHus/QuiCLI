namespace QuiCLI.Command;

public interface ICommandBuilder
{
    public IConfigureCommandInstance<TCommand> AddCommand<TCommand>(string command) where TCommand : class;
}

internal interface IBuildCommands
{
    IEnumerable<CommandDefinition> Build();
}
