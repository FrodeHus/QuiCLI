using QuiCLI.Builder;
using QuiCLI.Command;
using QuiCLI.Help;
using QuiCLI.Internal;
using System.Diagnostics.CodeAnalysis;

namespace QuiCLI;

public class QuicApp
{
    public required IServiceProvider ServiceProvider { get; init; }
    public required CommandGroup RootCommands { get; init; }
    internal List<ArgumentDefinition> GlobalArguments { get; init; } = [];
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
        if (RequestedHelp(parsedCommand))
        {
            var helpBuilder = new HelpBuilder(parsedCommand.CommandGroup!);
            return helpBuilder.BuildHelp(parsedCommand.Definition);
        }
        return await Dispatcher.InvokeAsync(commandInstance, parsedCommand);
    }

    internal static bool RequestedHelp(ParsedCommand command)
    {
        return command.Arguments.Any(a => a.Name == "help" && (bool)a.Value);
    }

    public void Run()
    {
        RunAsync().GetAwaiter().GetResult();
    }
    public async Task RunAsync()
    {

        var parser = new CommandLineParser(RootCommands);
        var (command, group) = parser.Parse(Environment.GetCommandLineArgs().Skip(1).ToArray());
        if (command is not null)
        {
            var (_, instance) = GetCommandInstance(command);
            var result = await GetCommandOutput(instance, command);

            Console.WriteLine(result?.ToString());
        }
        else
        {
            var helpBuilder = new HelpBuilder(group);
            Console.WriteLine(helpBuilder.BuildHelp());
        }
    }
}
