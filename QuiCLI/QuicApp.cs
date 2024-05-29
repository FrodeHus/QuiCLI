using QuiCLI.Builder;
using QuiCLI.Command;
using QuiCLI.Internal;
using System.Diagnostics.CodeAnalysis;

namespace QuiCLI;

public class QuicApp
{
    public required IServiceProvider ServiceProvider { get; init; }
    public CommandGroup RootCommands { get; init; } = new CommandGroup();
    public static QuicAppBuilder CreateBuilder()
    {
        return new QuicAppBuilder();
    }


    public QuicApp AddCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TCommand>(Func<IServiceProvider, TCommand> implementationFactory)
        where TCommand : class
    {
        RootCommands.AddCommand(implementationFactory);
        return this;
    }

    public CommandGroup AddCommandGroup(string name)
    {
        return RootCommands.AddCommandGroup(name);
    }

    internal (CommandDefinition, object) GetCommandInstance(ParsedCommand parsedCommand)
    {
        var (_, implementationFactory) = parsedCommand.CommandGroup!.GetCommand(parsedCommand.Name);
        return (parsedCommand.Definition, implementationFactory.Invoke(ServiceProvider));
    }

    internal async Task<object?> GetCommandOutput(object commandInstance, ParsedCommand parsedCommand)
    {
        return await Dispatcher.InvokeAsync(commandInstance, parsedCommand);
    }

    public void Run()
    {
        RunAsync().GetAwaiter().GetResult();
    }
    public async Task RunAsync()
    {

        var parser = new CommandLineParser(RootCommands);
        var command = parser.Parse(Environment.GetCommandLineArgs().Skip(1).ToArray());
        if (command is not null)
        {
            var (_, instance) = GetCommandInstance(command);
            var result = await GetCommandOutput(instance, command);

            Console.WriteLine(result?.ToString());
        }
        else
        {
            foreach (var cmd in RootCommands.Commands)
            {
                Console.WriteLine(cmd.Key.Name);
            }
        }
    }
}
