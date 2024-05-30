using Microsoft.Extensions.DependencyInjection;
using QuiCLI.Command;

namespace QuiCLI.Builder;

public class QuicAppBuilder
{
    public IServiceCollection Services { get; init; }
    internal List<ArgumentDefinition> GlobalArguments { get; init; } = [];
    public QuicAppBuilder()
    {
        Services = new ServiceCollection();
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
            DefaultValue = false,
            ValueType = typeof(bool)
        });

        GlobalArguments.Add(new ArgumentDefinition
        {
            Name = "version",
            InternalName = "version",
            Help = "Show version information",
            IsFlag = true,
            IsRequired = false,
            DefaultValue = false,
            ValueType = typeof(bool)
        });

        GlobalArguments.Add(new ArgumentDefinition
        {
            Name = "verbose",
            InternalName = "verbose",
            Help = "Show verbose output",
            IsFlag = true,
            IsRequired = false,
            DefaultValue = false,
            ValueType = typeof(bool)
        });
    }

    public QuicApp Build()
    {
        var provider = Services.BuildServiceProvider();

        return new QuicApp
        {
            ServiceProvider = provider,
            GlobalArguments = GlobalArguments,
            RootCommands = new CommandGroup() { GlobalArguments = GlobalArguments }
        };
    }
}
