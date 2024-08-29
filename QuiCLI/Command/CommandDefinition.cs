using System.Reflection;

namespace QuiCLI.Command
{
    public sealed class CommandDefinition(string name, string? description = null)
    {
        public string Name { get; init; } = name;
        public string? Help { get; init; } = description;
        public required List<ParameterDefinition> Parameters { get; init; }
        internal MethodInfo? Method { get; set; }


        public bool TryGetParameter(string name, out ParameterDefinition argumentDefinition)
        {
            if (Parameters.Any(a => a.Name == name))
            {
                argumentDefinition = Parameters.Single(a => a.Name == name);
                return true;
            }

            argumentDefinition = null!;
            return false;
        }
    }
}
