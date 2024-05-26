using Microsoft.Extensions.DependencyInjection;
using System;
namespace QuiCLI.Builder;
public class QuicServiceCollection
{
    private readonly IServiceCollection _services;
    private IServiceProvider _provider;

    public QuicServiceCollection()
    {
        _services = new ServiceCollection();
    }

    public void RegisterTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        _services.AddTransient<TService, TImplementation>();
    }

    public void RegisterSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        _services.AddSingleton<TService, TImplementation>();
    }

    public void BuildServiceProvider()
    {
        _provider = _services.BuildServiceProvider();
    }

    public TService ResolveService<TService>() where TService : class
    {
        return _provider.GetService<TService>();
    }
}
