using QuiCLI.Builder;

namespace QuiCLI.Tests.Builder
{
    public class CommandTests
    {
        [Fact]
        public void Fluent_CreateCommand()
        {
            // Arrange
            var builder = new QuicAppBuilder();
            builder.Commands.Add<TestCommand>()
                .WithCommand("test", x => x.Test);
            var app = builder.Build();

            Assert.Single(app.RootCommands.Commands);
            Assert.Equal("test", app.RootCommands.Commands[0].Name);
        }

        [Fact]
        public void Fluent_CreateCommandSubGroup()
        {
            var builder = new QuicAppBuilder();
            builder.Commands.WithGroup("cmds").Add<TestCommand>()
                .WithCommand("test", x => x.Test);

            var app = builder.Build();

            Assert.Single(app.RootCommands.SubGroups);
            Assert.Equal("cmds", app.RootCommands.SubGroups.First().Key);
            Assert.Single(app.RootCommands.SubGroups.First().Value.Commands);
        }
    }
}
