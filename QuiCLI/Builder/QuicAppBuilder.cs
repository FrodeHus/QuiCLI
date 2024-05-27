﻿using QuiCLI.Command;

namespace QuiCLI.Builder;

public class QuicAppBuilder
{
    private readonly QuicServiceCollection _serviceCollection;
    private readonly Dictionary<CommandDefinition, Func<IServiceProvider, object>> _commands = new();
    public QuicAppBuilder()
    {
        _serviceCollection = new QuicServiceCollection();
    }

    public QuicAppBuilder AddCommand<TCommand>(string name, Func<IServiceProvider, TCommand> implementationFactory)
        where TCommand : class
    {
        _commands.Add(new CommandDefinition(name), implementationFactory);
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
