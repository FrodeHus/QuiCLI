using Microsoft.Extensions.DependencyInjection;
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
    public ICommandBuilder Commands { get; init; }
    public IServiceCollection Services { get; init; }
    public IQuicPipelineBuilder Pipeline { get; init; }
    
    public QuicAppBuilder()
    {
        Services = new ServiceCollection();
        Commands = CommandBuilder.CreateBuilder(Services);
        Pipeline = new QuicPipelineBuilder()
            .UseMiddleware<ExceptionHandler>()
            .UseMiddleware<CommandDispatcher>()
            .UseMiddleware<StringOutputFormatter>();
        InitDefaultGlobalArguments();
    }

    internal void InitDefaultGlobalArguments()
    {

        Configuration.GlobalArguments.Add(new ParameterDefinition
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

        Configuration.GlobalArguments.Add(new ParameterDefinition
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

        var commandGroup = ((IBuildCommandGroup)Commands).Build();

        return new QuicApp
        {
            Configuration = Configuration,
            ServiceProvider = provider,
            Pipeline = Pipeline.Build(),
            RootCommands = commandGroup.First()
        };
    }
}
