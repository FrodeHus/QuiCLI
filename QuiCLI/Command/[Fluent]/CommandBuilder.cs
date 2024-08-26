using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Command;

public sealed class CommandBuilder : ICommandBuilder, IBuildCommands
{
    private readonly List<IBuilderState> _commands = [];
    private readonly IServiceCollection _services;

    private CommandBuilder(IServiceCollection services)
    {
        _services = services;
    }

    IConfigureCommandInstance<TCommand> ICommandBuilder.AddCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TCommand>(string command) where TCommand : class
    {
        _services.AddTransient<TCommand>();
        var state = new CommandBuilderState<TCommand>(command);
        _commands.Add(state);
        return state;
    }



    internal static ICommandBuilder CreateBuilder(IServiceCollection services)
    {
        return new CommandBuilder(services);
    }

    IEnumerable<CommandDefinition> IBuildCommands.Build()
    {
        return _commands.ConvertAll(c => (CommandDefinition)(c).Build());

    }
}
