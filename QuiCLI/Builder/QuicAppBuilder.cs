using Microsoft.Extensions.DependencyInjection;
using QuiCLI.Command;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace QuiCLI.Builder;

public class QuicAppBuilder
{
    public IServiceCollection Services { get; init; }
    public QuicAppBuilder()
    {
        Services = new ServiceCollection();
    }

    

    public QuicApp Build()
    {
        var provider = Services.BuildServiceProvider();

        return new QuicApp
        {
            ServiceProvider = provider
        };
    }
}
