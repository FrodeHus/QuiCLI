namespace QuiCLI.Command
{
    internal sealed class CommandDataSource
    {
        private readonly List<CommandDefinition> _commands = [];
        private CommandGroup? RootGroup { get; init; }
        public CommandDataSource(CommandGroup rootGroup)
        {
            RootGroup = rootGroup;
        }
        public void AddCommand(CommandDefinition command)
        {
            _commands.Add(command);
        }

        public CommandDefinition? GetCommand(string name)
        {
            return _commands.Find(c => c.Name == name);
        }
    }
}
