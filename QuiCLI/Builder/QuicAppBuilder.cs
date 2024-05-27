using QuiCLI.Command;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace QuiCLI.Builder;

public class QuicAppBuilder
{
    private readonly QuicServiceCollection _serviceCollection;
    private readonly Dictionary<CommandDefinition, Func<IServiceProvider, object>> _commands = new();
    public QuicAppBuilder()
    {
        _serviceCollection = new QuicServiceCollection();
    }

    public QuicAppBuilder AddCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TCommand>(Func<IServiceProvider, TCommand> implementationFactory)
        where TCommand : class
    {
        var methods = typeof(TCommand).GetMethods().Where(m => m.GetCustomAttribute<CommandAttribute>() is not null);

        foreach (var method in methods)
        {
            var commandAttribute = method.GetCustomAttribute<CommandAttribute>();
            if (commandAttribute is not null)
            {
                _commands.Add(new CommandDefinition(commandAttribute.Name), implementationFactory);
            }
        }
        return this;
    }

    public QuicAppBuilder RegisterTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        _serviceCollection.RegisterTransient<TService, TImplementation>();
        return this;
    }

    public QuicAppBuilder RegisterSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        _serviceCollection.RegisterSingleton<TService, TImplementation>();
        return this;
    }

    public QuicApp Build()
    {
        _serviceCollection.BuildServiceProvider();
        return new QuicApp
        {
            Commands = _commands
        };
    }
}
