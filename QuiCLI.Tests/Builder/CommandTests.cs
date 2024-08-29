using QuiCLI.Builder;
using QuiCLI.Command;

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

        [Fact]
        public void CommandBuilder_DetectFlagParameter()
        {
            var builder = QuicApp.CreateBuilder();
            builder.Commands.Add<TestCommand>()
                .WithCommand("test5", x => x.Test5);
            var app = builder.Build();
            var parser = new CommandLineParser(app.RootCommands, app.Configuration);
            var definition = app.RootCommands.Commands[0];

            Assert.True(definition.Parameters[0].IsFlag);
            Assert.False(definition.Parameters[0].IsRequired);
        }
    }
}
