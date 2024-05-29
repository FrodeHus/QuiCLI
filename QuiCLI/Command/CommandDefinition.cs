using System.Reflection;

namespace QuiCLI.Command
{
    public sealed class CommandDefinition(string name, string? description = null)
    {
        public string Name { get; init; } = name;
        public string? Description { get; init; } = description;
        public required List<ParameterDefinition> Parameters { get; init; }
        public MethodInfo? Method { get; set; }

        public ParameterDefinition? GetParameter(string name)
        {
            return Parameters.Find(o => o.Name == name);
        }
    }
}
