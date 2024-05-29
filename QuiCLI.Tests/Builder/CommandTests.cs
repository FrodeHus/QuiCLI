using QuiCLI.Builder;

namespace QuiCLI.Tests.Builder
{
    public class CommandTests
    {
        [Fact]
        public void AddCommand_ShouldAddCommand()
        {
            // Arrange
            var builder = new QuicAppBuilder();
            var app = builder.Build();


            // Act
            app.AddCommand(sp => new TestCommand());


            // Assert
            Assert.Equal(5, app.RootCommands.Commands.Count);
            Assert.Equal("test", app.RootCommands.Commands.First().Key.Name);
        }
    }
}
