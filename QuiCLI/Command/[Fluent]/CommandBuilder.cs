namespace QuiCLI.Command;

public sealed class CommandBuilder : ICommandBuilder
{
    private readonly List<IBuilderState> _commands = [];

    private CommandBuilder()
    {
    }

    internal IEnumerable<CommandDefinition> Build()
    {
        return _commands.ConvertAll(c => (CommandDefinition)(c).Build());
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
}
