namespace QuiCLI.Command;

public sealed class CommandBuilder : ICommandBuilder, IBuildCommands
{
    private readonly List<IBuilderState> _commands = [];

    private CommandBuilder()
    {
    }

    IConfigureCommandInstance<TCommand> ICommandBuilder.AddCommand<TCommand>(string command) where TCommand : class
    {
        var state = new CommandBuilderState<TCommand>(command);
        _commands.Add(state);
        return state;
    }



    internal static ICommandBuilder CreateBuilder()
    {
        return new CommandBuilder();
    }

    IEnumerable<CommandDefinition> IBuildCommands.Build()
    {
        return _commands.ConvertAll(c => (CommandDefinition)(c).Build());

    }
}
