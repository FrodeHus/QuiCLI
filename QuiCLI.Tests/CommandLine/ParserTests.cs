﻿using QuiCLI.Command;

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
        public void CommandLineParser_DiscoverParameters()
        {
            var commands = new CommandGroup();
            var command = commands.AddCommand(_ => new TestCommand()).First(c => c.Name == "test2");
            var commandLine = new string[] { "test2", "--name", "Foo" };
            var parser = new CommandLineParser(commands);
            var parsedCommands = parser.Parse(commandLine);
            Assert.Single(parsedCommands);
            Assert.Single(parsedCommands[0].Options);
            Assert.Equal("Foo", parsedCommands[0].Options[0].Value);
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
            var parsedCommands = parser.Parse(commandLine);
            Assert.Single(parsedCommands);
            Assert.Single(parsedCommands[0].Options);
            Assert.Equal(expected, parsedCommands[0].Options[0].Value);
        }

        [Fact]
        public void CommandLineParser_ParseGroups()
        {

        }
    }
}
