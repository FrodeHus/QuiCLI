namespace QuiCLI.Command
{
    internal sealed class CommandDataSource
    {
        private readonly List<CommandDefinition> _commands;

        public CommandDataSource()
        {
            _commands = [];
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
