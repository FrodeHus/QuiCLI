using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Middleware;

public sealed class QuicPipelineBuilder : IQuicPipelineBuilder
{
    private readonly List<MiddlewareDescriptor> _middlewares = [];
    public QuicMiddlewareDelegate Build()
    {
        QuicMiddlewareDelegate next = context => new ValueTask<int>(0);
        _middlewares.Reverse();
        foreach(var descriptor in _middlewares)
        {
            var middleware = CreateMiddlewareInstance(descriptor.MiddlewareType, next);
            next = middleware.OnExecute;
        }
        return next;
    }

    public IQuicPipelineBuilder UseMiddleware<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>() where T : QuicMiddleware
    {
        _middlewares.Add(new MiddlewareDescriptor(typeof(T)));
        return this;
    }

    private static QuicMiddleware CreateMiddlewareInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type middlewareType, QuicMiddlewareDelegate next)
    {
        return (QuicMiddleware)Activator.CreateInstance(middlewareType, next)!;
    }
}

internal sealed class MiddlewareDescriptor([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type middlewareType)
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private readonly Type _implementationType = middlewareType;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    public Type MiddlewareType => _implementationType;
}