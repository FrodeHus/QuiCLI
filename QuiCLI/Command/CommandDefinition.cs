using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace QuiCLI.Command
{
    public sealed class CommandDefinition(string name, string? description = null)
    {
        public string Name { get; init; } = name;
        public string? Help { get; init; } = description;
        public required List<ParameterDefinition> Arguments { get; init; }
        internal MethodInfo? Method { get; set; }


        public bool TryGetArgument(string name, out ParameterDefinition argumentDefinition)
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
