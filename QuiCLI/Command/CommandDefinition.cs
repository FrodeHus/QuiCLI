using System.Reflection;

namespace QuiCLI.Command
{
    public sealed class CommandDefinition(string name, string? description = null)
    {
        public string Name { get; init; } = name;
        public string? Description { get; init; } = description;
        public MethodInfo? Method { get; set; }
        public List<OptionDefinition> Options { get; init; } = [];
        public List<CommandDefinition> SubCommands { get; init; } = [];

        public OptionDefinition? GetOption(string name)
        {
            return Options.Find(o => o.Name == name);
        }

        public CommandDefinition AddIntegerOption(string name)
        {
            Options.Add(new OptionDefinition { Name = name, ValueType = typeof(int) });
            return this;
        }

        public CommandDefinition AddStringOption(string name)
        {
            Options.Add(new OptionDefinition { Name = name, ValueType = typeof(string) });
            return this;
        }

        public CommandDefinition AddBooleanOption(string name)
        {
            Options.Add(new OptionDefinition { Name = name, ValueType = typeof(bool), IsFlag = true });
            return this;
        }

        public CommandDefinition AddDoubleOption(string name)
        {
            Options.Add(new OptionDefinition { Name = name, ValueType = typeof(double) });
            return this;
        }

        public CommandDefinition AddOption(string name, Type type)
        {
            Options.Add(new OptionDefinition { Name = name, ValueType = type });
            return this;
        }
    }
}
