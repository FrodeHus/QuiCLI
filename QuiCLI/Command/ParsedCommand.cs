﻿namespace QuiCLI.Command
{
    public sealed class ParsedCommand
    {
        public string Name { get; init; }
        public CommandDefinition Definition { get; set; }
        public CommandGroup CommandGroup { get; set; }
        public List<ParameterValue> Arguments { get; } = [];

        public ParsedCommand(string name, CommandDefinition definition, CommandGroup group)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            Name = name;
            Definition = definition;
            CommandGroup = group;
        }

        public void AddArgument(ParameterDefinition argument, object value)
        {
            if (argument is null)
            {
                throw new ArgumentException($"'{nameof(argument)}' cannot be null or empty.", nameof(argument));
            }

            Arguments.Add(new ParameterValue(argument, value));
        }

        public bool ValidateArguments()
        {
            return Arguments.Any(arg => arg.Name.Equals("help", StringComparison.OrdinalIgnoreCase)) ||
                Definition
                .Parameters
                .Where(a => a.IsRequired && !a.IsGlobal)
                .All(a => Arguments.Exists(arg => arg.Argument == a));
        }
    }
}
