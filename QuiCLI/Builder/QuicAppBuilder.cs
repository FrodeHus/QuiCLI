using Microsoft.Extensions.DependencyInjection;
using QuiCLI.Command;
using QuiCLI.Middleware;
using QuiCLI.Output;
using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Builder;

public class QuicAppBuilder
{
    public IServiceCollection Services { get; init; }
    public IQuicPipelineBuilder Pipeline { get; init; }
    internal Dictionary<string, IOutputFormatter> OutputFormatters { get; init; } = new Dictionary<string, IOutputFormatter>
    {
        { "table", new TableFormatter() }
    };
    internal List<ArgumentDefinition> GlobalArguments { get; init; } = [];
    public QuicAppBuilder()
    {
        Services = new ServiceCollection();
        Pipeline = new QuicPipelineBuilder()
            .UseMiddleware<ExceptionHandler>()
            .UseMiddleware<CommandDispatcher>()
            .UseMiddleware<OutputFormatter>();
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

    public QuicAppBuilder AddOutputProvider<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TProvider>(string friendlyName) where TProvider : IOutputFormatter
    {
        if (OutputFormatters.ContainsKey(friendlyName))
        {
            throw new InvalidOperationException($"Output provider with name {friendlyName} already exists");
        }
        OutputFormatters.Add(friendlyName, Activator.CreateInstance<TProvider>());
        return this;
    }

    public QuicApp Build()
    {
        var provider = Services.BuildServiceProvider();

        return new QuicApp
        {
            ServiceProvider = provider,
            Pipeline = Pipeline.Build(),
            GlobalArguments = GlobalArguments,
            OutputFormatters = OutputFormatters,
            RootCommands = new CommandGroup() { GlobalArguments = GlobalArguments }
        };
    }
}
