namespace QuiCLI.Command
{
    public sealed class ParsedCommand
    {
        public string Name { get; init; }
        public CommandDefinition Definition { get; set; }
        public CommandGroup CommandGroup { get; set; }
        public List<ArgumentValue> Arguments { get; } = [];
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

        public void AddArgument(string argument, object value)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException($"'{nameof(argument)}' cannot be null or empty.", nameof(argument));
            }

            Arguments.Add(new ArgumentValue(argument, value));
        }
    }
}
