namespace QuiCLI.Help
{
    internal class HelpArgument(string name, bool isRequired, bool isFlag, string? help) : IHelpItem
    {
        public string Name { get; init; } = name;
        public string? Help { get; init; } = help;
        public bool IsRequired { get; init; } = isRequired;
        public bool IsFlag { get; init; } = isFlag;

        public override string ToString()
        {
            return $"{Name}{(IsRequired ? " (required)" : "")}{(IsFlag ? "" : " <value>")}\t:\t{Help}";
        }
    }
}