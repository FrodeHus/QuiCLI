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
            Assert.Equal(6, app.RootCommands.Commands.Count);
            Assert.Equal("test", app.RootCommands.Commands[0].Name);
        }

        [Fact]
        public void Fluent_CreateCommand()
        {
            // Arrange
            var builder = new QuicAppBuilder();
            builder.Commands.AddCommand<TestCommand>("test")
                .Configure(sp => new TestCommand(), x => x.Test);
            var app = builder.Build();

            Assert.Single(app.RootCommands.Commands);
            Assert.Equal("test", app.RootCommands.Commands[0].Name);
        }
    }
}
