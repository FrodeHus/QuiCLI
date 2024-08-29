using Microsoft.Extensions.DependencyInjection;
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
        context.CommandResult = await GetCommandOutput(instance, context.Command, context.Configuration);
        if (context.CommandResult is IAsyncResult)
        {
            var resultProperty = context.CommandResult?.GetType().GetProperty("Result");
            if (resultProperty is not null)
            {
                context.CommandResult = resultProperty?.GetValue(context.CommandResult);
            }
            else
            {
                throw new Exception("Command method returned async result, but could not determine value.");
            }
        }
        return await Next(context);
    }

    internal static (CommandDefinition, object) GetCommandInstance(ParsedCommand parsedCommand, IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        return (parsedCommand.Definition, scope.ServiceProvider.GetRequiredService(parsedCommand.Definition.Method!.DeclaringType!));
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
