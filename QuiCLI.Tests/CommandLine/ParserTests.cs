using QuiCLI.Command;

namespace QuiCLI.Tests.CommandLine
{
    public class ParserTests
    {
        [Fact]
        public void CommandLineParser_SimpleParse()
        {
            var commandLine = new string[] { "test", "test2", "test3" };
            var parser = new CommandLineParser();
            var commands = parser.Parse(commandLine);
            Assert.Equal(3, commands.Count);
        }

        [Fact]
        public void CommandLineParser_EmptyParse()
        {
            var commandLine = new string[] { };
            var parser = new CommandLineParser();
            var commands = parser.Parse(commandLine);
            Assert.Empty(commands);
        }

        [Fact]
        public void CommandLineParser_NullParse()
        {
            var parser = new CommandLineParser();
            var commands = parser.Parse(null!);
            Assert.Empty(commands);
        }
        [Fact]
        public void CommandLineParser_NullElementParse()
        {
            var commandLine = new string[] { "test", null!, "test3" };
            var parser = new CommandLineParser();
            var commands = parser.Parse(commandLine);
            Assert.Equal(2, commands.Count);
        }

        [Fact]
        public void CommandLineParser_EmptyElementParse()
        {
            var commandLine = new string[] { "test", "", "test3" };
            var parser = new CommandLineParser();
            var commands = parser.Parse(commandLine);
            Assert.Equal(2, commands.Count);
        }

        [Fact]
        public void CommandLineParser_WhitespaceElementParse()
        {
            var commandLine = new string[] { "test", " ", "test3" };
            var parser = new CommandLineParser();
            var commands = parser.Parse(commandLine);
            Assert.Equal(2, commands.Count);
        }

        [Fact]
        public void CommandLineParser_ParseOptions()
        {
            var commandLine = new string[] { "test", "--option1", "--option2" };
            var parser = new CommandLineParser();
            var commands = parser.Parse(commandLine);
            Assert.Single(commands);
            Assert.Equal(2, commands[0].Options.Count);
        }

        [Fact]
        public void CommandLineParser_ParseOptionsWithValues()
        {
            var commandLine = new string[] { "test", "--option1", "value1", "--option2", "value2" };
            var parser = new CommandLineParser();
            var commands = parser.Parse(commandLine);
            Assert.Single(commands);
            Assert.Equal(2, commands[0].Options.Count);
            Assert.Equal("value1", commands[0].Options[0].Value);
            Assert.Equal("value2", commands[0].Options[1].Value);
        }

        [Fact]
        public void CommandLineParser_ParseOptionsWithEqualsValue()
        {
            var commandLine = new string[] { "test", "--option1=value1", "--option2=value2" };
            var parser = new CommandLineParser();
            var commands = parser.Parse(commandLine);
            Assert.Single(commands);
            Assert.Equal(2, commands[0].Options.Count);
            Assert.Equal("value1", commands[0].Options[0].Value);
            Assert.Equal("value2", commands[0].Options[1].Value);
        }

        [Fact]
        public void CommandLineParser_ParseOptionsWithEqualsValueAndSpace()
        {
            var commandLine = new string[] { "test", "--option1= value1", "--option2 =value2" };
            var parser = new CommandLineParser();
            var commands = parser.Parse(commandLine);
            Assert.Single(commands);
            Assert.Equal(2, commands[0].Options.Count);
            Assert.Equal("value1", commands[0].Options[0].Value);
            Assert.Equal("value2", commands[0].Options[1].Value);
        }

        [Fact]
        public void CommandLineParser_ParseFlags()
        {
            var commands = new CommandDataSource();
            commands.AddCommand(new CommandDefinition("test")
            {
                Options = [new OptionDefinition { Name = "flag1", IsFlag = true, ValueType = typeof(bool) }]
            });
            var commandLine = new string[] { "test", "--flag1" };

            var parser = new CommandLineParser(commands);
            var parsedCommands = parser.Parse(commandLine);

            Assert.Single(parsedCommands);
            Assert.Single(parsedCommands[0].Options);
            Assert.True((bool)parsedCommands[0].Options[0].Value);
        }

        [Fact]
        public void CommandLineParser_ParseNumericOptions()
        {
            var commands = new CommandDataSource();
            var command = new CommandDefinition("test");
            command.AddIntegerOption("number1");
            commands.AddCommand(command);
            var commandLine = new string[] { "test", "--number1", "42" };
            var parser = new CommandLineParser(commands);
            var parsedCommands = parser.Parse(commandLine);
            Assert.Single(parsedCommands);
            Assert.Single(parsedCommands[0].Options);
            Assert.Equal(42, parsedCommands[0].Options[0].Value);
        }

        [Fact]
        public void CommandLineParser_ParseDoubleOptions()
        {
            var commands = new CommandDataSource();
            var command = new CommandDefinition("test");
            command.AddDoubleOption("number1");
            commands.AddCommand(command);
            var commandLine = new string[] { "test", "--number1", "42.42" };
            var parser = new CommandLineParser(commands);
            var parsedCommands = parser.Parse(commandLine);
            Assert.Single(parsedCommands);
            Assert.Single(parsedCommands[0].Options);
            Assert.Equal(42.42, parsedCommands[0].Options[0].Value);
        }

        [Fact]
        public void CommandLineParser_HandleIntAsDoubles()
        {
            var commands = new CommandDataSource();
            var command = new CommandDefinition("test");
            command.AddDoubleOption("number1");
            commands.AddCommand(command);
            var commandLine = new string[] { "test", "--number1", "42" };
            var parser = new CommandLineParser(commands);
            var parsedCommands = parser.Parse(commandLine);
            Assert.Single(parsedCommands);
            Assert.Single(parsedCommands[0].Options);
            Assert.Equal(42.0, parsedCommands[0].Options[0].Value);
        }
    }
}
