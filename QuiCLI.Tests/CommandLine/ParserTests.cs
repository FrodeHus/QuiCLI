using QuiCLI.Command;

namespace QuiCLI.Tests.CommandLine
{
    public class ParserTests
    {

        [Fact]
        public void CommandLineParser_ParseOptionsWithEqualsValueAndSpace()
        {
            var commandGroup = new CommandGroup()
            {
                GlobalArguments = [new ArgumentDefinition { Name = "help", InternalName = "help" }]
            };
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
            var commands = new CommandGroup()
            {
                GlobalArguments = [new ArgumentDefinition { Name = "help", InternalName = "help" }]
            };
            var command = commands.AddCommand(_ => new TestCommand()).First(c => c.Name == "test2");
            var commandLine = new string[] { "test2", "--parameter", "Foo" };
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
            var commands = new CommandGroup()
            {
                GlobalArguments = [new ArgumentDefinition { Name = "help", InternalName = "help" }]
            };
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
            var commands = new CommandGroup()
            {
                GlobalArguments = [new ArgumentDefinition { Name = "help", InternalName = "help" }]
            };
            commands.AddCommandGroup("group1").AddCommand(_ => new TestCommand()).First(c => c.Name == "test2");
            var commandLine = new string[] { "group1", "test2", "--parameter", "test" };
            var parser = new CommandLineParser(commands);
            var (parsedCommand, _) = parser.Parse(commandLine);
            Assert.NotNull(parsedCommand);
            Assert.Single(parsedCommand.Arguments);
            Assert.Equal("test", parsedCommand.Arguments[0].Value);
        }

        [Fact]
        public void CommandLineParser_DetectGlobalArguments()
        {
            var commands = new CommandGroup()
            {
                GlobalArguments = [new ArgumentDefinition { Name = "help", InternalName = "help" }]
            };
            var command = commands.AddCommand(_ => new TestCommand()).First(c => c.Name == "test2");
            var commandLine = new string[] { "test2", "--parameter", "test", "--help" };
            var parser = new CommandLineParser(commands);
            var (parsedCommand, _) = parser.Parse(commandLine);
            Assert.NotNull(parsedCommand);
            Assert.Equal(2, parsedCommand.Arguments.Count);
            Assert.Contains(parsedCommand.Arguments, a => a.Name == "help");
        }

        [Fact]
        public void CommandLineParser_DetectOptionalArguments()
        {
            var commands = new CommandGroup()
            {
                GlobalArguments = [new ArgumentDefinition { Name = "help", InternalName = "help" }]
            };
            var command = commands.AddCommand(_ => new TestCommand()).First(c => c.Name == "test6");
            var optionalArgument = command.Arguments.SingleOrDefault(a => a.Name == "parameter2");
            Assert.NotNull(optionalArgument);
            Assert.False(optionalArgument.IsRequired);
        }
        [Fact]
        public void CommandLineParser_DetectRequiredArguments()
        {
            var commands = new CommandGroup()
            {
                GlobalArguments = [new ArgumentDefinition { Name = "help", InternalName = "help" }]
            };
            var command = commands.AddCommand(_ => new TestCommand()).First(c => c.Name == "test6");
            var optionalArgument = command.Arguments.SingleOrDefault(a => a.Name == "parameter");
            Assert.NotNull(optionalArgument);
            Assert.True(optionalArgument.IsRequired);
        }
    }
}
