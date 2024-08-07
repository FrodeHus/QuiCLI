﻿using Microsoft.Extensions.DependencyInjection;
using QuiCLI.Command;
using QuiCLI.Middleware;

namespace QuiCLI.Builder;

public class QuicAppBuilder
{
    public void Configure(Action<Configuration> configureDelegate)
    {
        configureDelegate(Configuration);
    }

    internal Configuration Configuration { get; } = new Configuration();
    public IServiceCollection Services { get; init; }
    public IQuicPipelineBuilder Pipeline { get; init; }
    internal List<ArgumentDefinition> GlobalArguments { get; init; } = [];
    
    public QuicAppBuilder()
    {
        Services = new ServiceCollection();
        Pipeline = new QuicPipelineBuilder()
            .UseMiddleware<ExceptionHandler>()
            .UseMiddleware<CommandDispatcher>()
            .UseMiddleware<StringOutputFormatter>();
        InitDefaultGlobalArguments();
    }

    internal void InitDefaultGlobalArguments()
    {

        GlobalArguments.Add(new ArgumentDefinition
        {
            Name = "help",
            InternalName = "help",
            Help = "Show help information",
            IsFlag = true,
            IsRequired = false,
            IsGlobal = true,
            DefaultValue = false,
            ValueType = typeof(bool)
        });

        GlobalArguments.Add(new ArgumentDefinition
        {
            Name = "output",
            InternalName = "output",
            Help = "Output format",
            IsFlag = false,
            IsRequired = false,
            IsGlobal = true,
            DefaultValue = "string",
            ValueType = typeof(String)
        });
    }

    public QuicApp Build()
    {
        var provider = Services.BuildServiceProvider();

        return new QuicApp
        {
            Configuration = Configuration,
            ServiceProvider = provider,
            Pipeline = Pipeline.Build(),
            GlobalArguments = GlobalArguments,
            RootCommands = new CommandGroup() { GlobalArguments = GlobalArguments }
        };
    }
}
