namespace QuiCLI.Command
{
    internal class CommandDefinition(string name, string? description = null)
    {
        public string Name { get; init; } = name;
        public string? Description { get; init; } = description;
        public List<OptionDefinition> Options { get; init; } = [];
        public List<CommandDefinition> SubCommands { get; init; } = [];

        public OptionDefinition? GetOption(string name)
        {
            return Options.Find(o => o.Name == name);
        }

        public void AddIntegerOption(string name)
        {
            Options.Add(new OptionDefinition { Name = name, ValueType = typeof(int)});
        }

        public void AddStringOption(string name)
        {
            Options.Add(new OptionDefinition { Name = name, ValueType = typeof(string) });
        }

        public void AddBooleanOption(string name)
        {
            Options.Add(new OptionDefinition { Name = name, ValueType = typeof(bool), IsFlag = true });
        }

        public void AddDoubleOption(string name)
        {
            Options.Add(new OptionDefinition { Name = name, ValueType = typeof(double) });
        }

        public void AddOption(string name, Type type)
        {
            Options.Add(new OptionDefinition { Name = name, ValueType = type });
        }
    }
}
