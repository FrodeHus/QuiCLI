using QuiCLI.Builder;
using QuiCLI.Command;
using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Middleware;

public delegate ValueTask<int> QuicMiddlewareDelegate(QuicCommandContext context);
public class QuicCommandContext(ParsedCommand command, Configuration configuration)
{
    public ParsedCommand Command { get; } = command;
    public Configuration Configuration { get; } = configuration;
    public List<ParameterValue> GlobalArguments { get; init; } = [];
    public required IServiceProvider ServiceProvider { get; init; }
    public object? CommandResult { get; internal set; }
}

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
