using QuiCLI.Command;
using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Middleware;

public delegate ValueTask<int> QuicMiddlewareDelegate(QuicCommandContext context);

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public abstract class QuicMiddleware
{

    protected QuicMiddleware(QuicMiddlewareDelegate next)
    {
        Next = next;
    }
    public QuicMiddlewareDelegate Next { get; }

    public abstract ValueTask<int> OnExecute(QuicCommandContext context);
}
