using QuiCLI.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiCLI.Tests.App;

public class ExecutionTests
{
    [Fact]
    public void ExecuteCommand_ShouldInstantiateCommand()
    {
        // Arrange
        var builder = new QuicAppBuilder();
        var app = builder.Build();
        app.AddCommand(sp => new TestCommand());

        var commandLine = new string[] { "test" };

        // Act
        var (_, instance) = app.GetCommandInstance("test");

        // Assert
        Assert.IsType<TestCommand>(instance);
    }

    [Fact]
    public async Task ExecuteCommand_ShouldExecuteCommand()
    {
        // Arrange
        var builder = new QuicAppBuilder();
        var app = builder.Build();
        app.AddCommand(sp => new TestCommand());

        var commandLine = new string[] { "test" };

        // Act
        var (definition, instance) = app.GetCommandInstance("test");
        var result = await app.GetCommandOutput(instance, definition);

        // Assert
        Assert.Equal("Hello, world!", result);
    }
}
