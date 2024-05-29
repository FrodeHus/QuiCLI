using QuiCLI.Command;

namespace QuiCLI.Tests.CommandLine
{
    public class ParserTests
    {

        [Fact]
        public void CommandLineParser_ParseOptionsWithEqualsValueAndSpace()
        {
            var commandGroup = new CommandGroup();
            var command = commandGroup.AddCommand(_ => new TestCommand()).First(c => c.Name == "test2");
            var commandLine = new string[] { "test2", "--parameter= world" };
            var parser = new CommandLineParser(commandGroup);
            var (parsedCommand, _) = parser.Parse(commandLine);
            Assert.NotNull(parsedCommand);
            Assert.Single(parsedCommand.Arguments);
            Assert.Equal("world", parsedCommand.Arguments[0].Value);
        }

        [Fact]
        public void CommandLineParser_DiscoverParameters()
        {
            var commands = new CommandGroup();
            var command = commands.AddCommand(_ => new TestCommand()).First(c => c.Name == "test2");
            var commandLine = new string[] { "test2", "--name", "Foo" };
            var parser = new CommandLineParser(commands);
            var (parsedCommand, _) = parser.Parse(commandLine);
            Assert.NotNull(parsedCommand);
            Assert.Single(parsedCommand.Arguments);
            Assert.Equal("Foo", parsedCommand.Arguments[0].Value);
        }

        [Theory]
        [InlineData("test2", "42", "42")]
        [InlineData("test3", "42", 42)]
        [InlineData("test4", "42.42", 42.42)]
        [InlineData("test5", null, true)]
        public void CommandLineParser_ParseParameterValues(string commandName, object? value, object expected)
        {
            var commands = new CommandGroup();
            var command = commands.AddCommand(_ => new TestCommand()).First(c => c.Name == commandName);
            var commandLine = new string[] { commandName, "--parameter", value?.ToString()! };
            var parser = new CommandLineParser(commands);
            var (parsedCommand, _) = parser.Parse(commandLine);
            Assert.NotNull(parsedCommand);
            Assert.Single(parsedCommand.Arguments);
            Assert.Equal(expected, parsedCommand.Arguments[0].Value);
        }

        [Fact]
        public void CommandLineParser_ParseGroups()
        {
            var commands = new CommandGroup();
            commands.AddCommandGroup("group1").AddCommand(_ => new TestCommand()).First(c => c.Name == "test2");
            var commandLine = new string[] { "group1", "test2", "--parameter", "test" };
            var parser = new CommandLineParser(commands);
            var (parsedCommand, _) = parser.Parse(commandLine);
            Assert.NotNull(parsedCommand);
            Assert.Single(parsedCommand.Arguments);
            Assert.Equal("test", parsedCommand.Arguments[0].Value);
        }
    }
}
