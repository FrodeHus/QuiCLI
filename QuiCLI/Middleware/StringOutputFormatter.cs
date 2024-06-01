namespace QuiCLI.Middleware
{
    internal class StringOutputFormatter(QuicMiddlewareDelegate next) : QuicMiddleware(next)
    {
        public override async ValueTask<int> OnExecute(QuicCommandContext context)
        {
            context.CommandResult = context.CommandResult?.ToString();
            return await Next(context);
        }
    }
}
