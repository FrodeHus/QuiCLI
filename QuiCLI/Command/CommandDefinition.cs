using System.Reflection;

namespace QuiCLI.Command
{
    public sealed class CommandDefinition(string name, string? description = null)
    {
        public string Name { get; init; } = name;
        public string? Help { get; init; } = description;
        public required List<ArgumentDefinition> Arguments { get; init; }
        public MethodInfo? Method { get; set; }

        public bool TryGetArgument(string name, out ArgumentDefinition argumentDefinition)
        {
            if (Arguments.Any(a => a.Name == name))
            {
                argumentDefinition = Arguments.Single(a => a.Name == name);
                return true;
            }

            argumentDefinition = null!;
            return false;
        }
    }
}
