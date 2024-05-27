namespace QuiCLI.Command
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CommandAttribute : Attribute
    {
        public string Name { get; }
        public string? Description { get; }

        public CommandAttribute(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }
    }
}
