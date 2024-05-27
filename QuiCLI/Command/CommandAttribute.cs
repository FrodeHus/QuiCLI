namespace QuiCLI.Command
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CommandAttribute : Attribute
    {
        public string Name { get; }
        public string? Help { get; }

        public CommandAttribute(string name, string? help = null)
        {
            Name = name;
            Help = help;
        }
    }
}
