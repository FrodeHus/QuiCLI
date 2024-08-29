namespace QuiCLI.Command
{
    public class CommandGroup(string? name = null)
    {
        public string? Name { get; init; } = name;

        public List<CommandDefinition> Commands
        {
            get; internal set;
        } = [];

        public Dictionary<string, CommandGroup> SubGroups
        {
            get; internal set;
        } = [];

        public CommandDefinition? GetCommand(string name)
        {
            return Commands.Find(c => c.Name == name);
        }
    }
}
