using QuiCLI.Command;

namespace QuiCLI.Tests.CommandLine
{
    public class ParserTests
    {

        [Fact]
        public void CommandLineParser_ParseOptionsWithEqualsValueAndSpace()
        {
            var builder = QuicApp.CreateBuilder();
            builder.Commands.Add<TestCommand>()
                .WithCommand("test2", x => x.Test2);
            var app = builder.Build();

            var commandLine = new string[] { "test2", "--parameter= world" };
            var parser = new CommandLineParser(app.RootCommands, builder.Configuration);
            var result = parser.Parse(commandLine);
            Assert.True(result.IsSuccess);
            var parsedCommand = result.Value.ParsedCommand;
            Assert.NotNull(parsedCommand);
            Assert.Single(parsedCommand.Arguments);
            Assert.Equal("world", parsedCommand.Arguments[0].Value);
        }

        [Fact]
        public void CommandLineParser_DiscoverParameters()
        {
            var builder = QuicApp.CreateBuilder();
            builder.Commands.Add<TestCommand>()
                .WithCommand("test2", x => x.Test2);
            var app = builder.Build();
            var parser = new CommandLineParser(app.RootCommands, app.Configuration);

            var commandLine = new string[] { "test2", "--parameter", "Foo" };
            var result = parser.Parse(commandLine);
            Assert.True(result.IsSuccess);
            var parsedCommand = result.Value.ParsedCommand;
            Assert.NotNull(parsedCommand);
            Assert.Single(parsedCommand.Arguments);
            Assert.Equal("Foo", parsedCommand.Arguments[0].Value);
        }

        [Fact]
        public void CommandLineParser_ParseGroups()
        {
            var builder = QuicApp.CreateBuilder();
            builder.Commands.WithGroup("group1")
                .Add<TestCommand>()
                .WithCommand("test2", x => x.Test2);
            var app = builder.Build();
            var parser = new CommandLineParser(app.RootCommands, app.Configuration);

            var commandLine = new string[] { "group1", "test2", "--parameter", "test" };
            var result = parser.Parse(commandLine);
            Assert.True(result.IsSuccess);
            var parsedCommand = result.Value.ParsedCommand;
            Assert.NotNull(parsedCommand);
            Assert.Single(parsedCommand.Arguments);
            Assert.Equal("test", parsedCommand.Arguments[0].Value);
        }

        [Fact]
        public void CommandLineParser_DetectGlobalArguments()
        {
            var builder = QuicApp.CreateBuilder();
            builder.Commands.Add<TestCommand>()
                .WithCommand("test2", x => x.Test2);
            var app = builder.Build();
            var parser = new CommandLineParser(app.RootCommands, app.Configuration);

            var commandLine = new string[] { "test2", "--parameter", "test", "--help" };
            var result = parser.Parse(commandLine);
            Assert.True(result.IsSuccess);
            var parsedCommand = result.Value.ParsedCommand;
            Assert.NotNull(parsedCommand);
            Assert.Equal(2, parsedCommand.Arguments.Count);
            Assert.Contains(parsedCommand.Arguments, a => a.Name == "help");
        }

        [Fact]
        public void CommandLineParser_DetectOptionalArguments()
        {
            var builder = QuicApp.CreateBuilder();
            builder.Commands.Add<TestCommand>()
                .WithCommand("test6", x => x.Test6);
            var app = builder.Build();

            var optionalArgument = app.RootCommands.Commands[0].Arguments.SingleOrDefault(a => a.Name == "parameter2");
            Assert.NotNull(optionalArgument);
            Assert.False(optionalArgument.IsRequired);
        }
        [Fact]
        public void CommandLineParser_DetectRequiredArguments()
        {
            var builder = QuicApp.CreateBuilder();
            builder.Commands.Add<TestCommand>()
                .WithCommand("test6", x => x.Test6);
            var app = builder.Build();

            var optionalArgument = app.RootCommands.Commands[0].Arguments.SingleOrDefault(a => a.Name == "parameter");
            Assert.NotNull(optionalArgument);
            Assert.True(optionalArgument.IsRequired);
        }

        [Fact]
        public void CommandLineParser_DetectMissingRequiredArguments()
        {
            var builder = QuicApp.CreateBuilder();
            builder.Commands.Add<TestCommand>()
                .WithCommand("test6", x => x.Test6);
            var app = builder.Build();
            var parser = new CommandLineParser(app.RootCommands, app.Configuration);

            var commandLine = new string[] { "test6" };
            var result = parser.Parse(commandLine);
            Assert.True(result.IsFailure);
        }
    }
}
