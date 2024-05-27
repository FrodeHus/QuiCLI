using QuiCLI.Builder;
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

    internal object GetCommandInstance(string name)
    {
        var command = RootCommands.Commands.FirstOrDefault(c => c.Key.Name == name);
        return command.Value.Invoke(ServiceProvider);
    }

    public void Run()
    {

    }
    public Task RunAsyc()
    {
        return Task.CompletedTask;
    }
}
