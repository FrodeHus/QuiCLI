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

        // Act
        var pipeline = builder.Build();
        var context = new QuicCommandContext(null!) { ServiceProvider = null! };
        var result = await pipeline(context);
        // Assert
        Assert.NotNull(pipeline);
        Assert.Equal(0, result);
        Assert.Equal("Hello, world!", context.CommandResult);
    }

    public class TestMiddleware(QuicMiddlewareDelegate next) : QuicMiddleware(next)
    {
        public override async ValueTask<int> OnExecute(QuicCommandContext context)
        {
            context.CommandResult = "Hello, world!";
            return await Next(context);
        }
    }
}
