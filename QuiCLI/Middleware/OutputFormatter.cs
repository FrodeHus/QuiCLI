namespace QuiCLI.Middleware;

internal class OutputFormatter(QuicMiddlewareDelegate next) : QuicMiddleware(next)
{
    public override async ValueTask<int> OnExecute(QuicCommandContext context)
    {
        var outputProviderName = context.GlobalArguments.First(a => a.Argument.Name == "output")?.Value as string;
        if (string.IsNullOrEmpty(outputProviderName))
        {
            context.CommandResult = context.CommandResult?.ToString();
        }
        else
        {
            var outputFormatter = context.OutputFormatters[outputProviderName.ToString()];
        }
        return await Next(context);
    }
}
