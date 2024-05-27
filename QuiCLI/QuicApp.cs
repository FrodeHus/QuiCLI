using QuiCLI.Builder;
using QuiCLI.Command;
using System.Diagnostics.CodeAnalysis;

namespace QuiCLI;

public class QuicApp
{
    public required IServiceProvider ServiceProvider { get; init; }
    public CommandGroup RootCommands { get; init; } = new CommandGroup();
    public Dictionary<string, CommandGroup> CommandGroups { get; init; } = [];
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
        var group = new CommandGroup();
        CommandGroups.Add(name, group);
        return group;
    }
    internal (CommandDefinition, object) GetCommandInstance(string name)
    {
        var command = RootCommands.Commands.FirstOrDefault(c => c.Key.Name == name);
        return (command.Key, command.Value.Invoke(ServiceProvider));
    }

    internal async Task<object?> GetCommandOutput(object commandInstance, CommandDefinition definition)
    {
        var result = definition.Method?.Invoke(commandInstance, null);
        if (result is Task task)
        {
            await task.ConfigureAwait(false);
            var property = Type.GetType("System.Threading.Tasks.Task")?.GetProperty("Result");
            return property?.GetValue(task);
        }
        return result;
    }

    public void Run()
    {
        RunAsync().GetAwaiter().GetResult();
    }
    public async Task RunAsync()
    {

        var parser = new CommandLineParser();
        var commands = parser.Parse(Environment.GetCommandLineArgs().Skip(1).ToArray());
        if (commands.Any())
        {
            var (definition, instance) = GetCommandInstance(commands[0].Name);
            var result = await GetCommandOutput(instance, definition);
            
            Console.WriteLine(result?.ToString());
        }
        else
         if (!commands.Any())
        {
            foreach (var command in RootCommands.Commands)
            {
                Console.WriteLine(command.Key.Name);
            }
        }
    }
}
