using QuiCLI.Command;
using QuiCLI.Output;
using System.Diagnostics.CodeAnalysis;

namespace QuiCLI.Middleware;
public class QuicCommandContext(ParsedCommand command)
{
    public ParsedCommand Command { get; } = command;
    public List<ArgumentValue> GlobalArguments { get; init; } = [];
    public required IServiceProvider ServiceProvider { get; init; }
    public Dictionary<string, IOutputFormatter> OutputFormatters { get; init; } = [];
    public object? CommandResult { get; internal set; }
}
