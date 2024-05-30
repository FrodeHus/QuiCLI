﻿using QuiCLI.Command;
using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Middleware;

public delegate ValueTask<int> QuicMiddlewareDelegate(QuicCommandContext context);
public class QuicCommandContext(ParsedCommand command)
{
    public ParsedCommand Command { get; } = command;
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
