using QuiCLI.Middleware;

namespace QuiCLI.Tests.Middleware;

public class MiddlewareTests
{
    [Fact]
    public async Task Middleware_SetResult()
    {
        // Arrange
        var builder = new QuicPipelineBuilder();
        builder.UseMiddleware<TestMiddleware>();
        builder.UseMiddleware<TestMiddleware2>();

        // Act
        var pipeline = builder.Build();
        var context = new QuicCommandContext(null!, new QuiCLI.Builder.Configuration()) { ServiceProvider = null! };
        var result = await pipeline(context);
        // Assert
        Assert.NotNull(pipeline);
        Assert.Equal(0, result);
        Assert.Equal("Hello, world!", context.CommandResult!);
    }

    public class TestMiddleware(QuicMiddlewareDelegate next) : QuicMiddleware(next)
    {
        public override async ValueTask<int> OnExecute(QuicCommandContext context)
        {
            context.CommandResult = "Hello,";
            return await Next(context);
        }
    }

    public class TestMiddleware2(QuicMiddlewareDelegate next) : QuicMiddleware(next)
    {
        public override async ValueTask<int> OnExecute(QuicCommandContext context)
        {
            if (context.CommandResult is not null)
                context.CommandResult += " world!";
            return await Next(context);
        }
    }
}
