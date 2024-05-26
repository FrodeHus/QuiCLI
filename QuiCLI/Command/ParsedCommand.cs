namespace QuiCLI.Command
{
    public sealed class ParsedCommand
    {
        public string Name { get; init; }
        public List<OptionValue> Options { get; } = [];
        public ParsedCommand(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            Name = name;
        }

        public void AddOption(string option, object value)
        {
            if (string.IsNullOrEmpty(option))
            {
                throw new ArgumentException($"'{nameof(option)}' cannot be null or empty.", nameof(option));
            }

            Options.Add(new OptionValue(option, value));
        }
    }
}
