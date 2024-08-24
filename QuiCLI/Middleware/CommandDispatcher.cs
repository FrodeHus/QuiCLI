using QuiCLI.Builder;
using QuiCLI.Command;
using QuiCLI.Help;
using QuiCLI.Internal;

namespace QuiCLI.Middleware;

internal sealed class CommandDispatcher(QuicMiddlewareDelegate next) : QuicMiddleware(next)
{
    public override async ValueTask<int> OnExecute(QuicCommandContext context)
    {
        var (_, instance) = GetCommandInstance(context.Command, context.ServiceProvider);
        var result = await GetCommandOutput(instance, context.Command, context.Configuration);
        context.CommandResult = result;
        return await Next(context);
    }

    internal static (CommandDefinition, object) GetCommandInstance(ParsedCommand parsedCommand, IServiceProvider serviceProvider)
    {
        return (parsedCommand.Definition, parsedCommand.Definition.ImplementationFactory!.Invoke(serviceProvider));
    }

    internal async Task<object?> GetCommandOutput(object commandInstance, ParsedCommand parsedCommand, Configuration configuration)
    {
        if (RequestedHelp(parsedCommand))
        {
            var helpBuilder = new HelpBuilder(parsedCommand.CommandGroup!, configuration);
            return helpBuilder.BuildHelp(parsedCommand.Definition);
        }
        return await Dispatcher.InvokeAsync(commandInstance, parsedCommand);
    }
    internal static bool RequestedHelp(ParsedCommand command)
    {
        return command.Arguments.Any(a => a.Name == "help" && (bool)a.Value);
    }
}
