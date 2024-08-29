using QuiCLI.Builder;
using QuiCLI.Command;
using System.Text;

namespace QuiCLI.Help;

internal class HelpBuilder(CommandGroup rootCommandGroup, Configuration configuration)
{
    private readonly CommandGroup _rootCommandGroup = rootCommandGroup;
    private readonly Configuration _configuration = configuration;

    public string BuildHelp()
    {
        var sb = new StringBuilder();
        if (_configuration.CustomBanner is not null)
        {
            sb.AppendLine(_configuration.CustomBanner());
        }
        var groupName = _rootCommandGroup.Name ?? "";
        sb.AppendLine("Usage:");
        sb.AppendLine($" {groupName} <command> [arguments]");
        sb.AppendLine();
        sb.AppendLine("Commands:");
        foreach (var command in _rootCommandGroup.Commands)
        {
            sb.Append($"\t{command.Name}");
            if (!string.IsNullOrWhiteSpace(command.Help))
            {
                sb.AppendLine($"\t:\t{command.Help}");
            }
            else
            {
                sb.AppendLine();
            }
        }

        sb.AppendLine();
        if (_rootCommandGroup.SubGroups.Count > 0)
        {
            sb.AppendLine("Nested Commands:");
            foreach (var group in _rootCommandGroup.SubGroups)
            {
                sb.AppendLine($"\t{group.Key}");
            }
        }
        if (_configuration.GlobalArguments.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("Global Arguments:");
            var maxArgLength = _configuration.GlobalArguments.Max(a => a.Name.Length);

            foreach (var argument in _configuration.GlobalArguments)
            {
                sb.AppendLine($"\t--{argument.Name.PadRight(maxArgLength)}\t:\t{argument.Help}");
            }
        }
        return sb.ToString();
    }

    public string BuildHelp(CommandDefinition commandDefinition)
    {
        var sb = new StringBuilder();
        if (_configuration.CustomBanner is not null)
        {
            sb.AppendLine(_configuration.CustomBanner());
        }

        sb.AppendLine($"Usage: {commandDefinition.Name}");
        if (!string.IsNullOrWhiteSpace(commandDefinition.Help))
        {
            sb.AppendLine(commandDefinition.Help);
        }
        sb.AppendLine();
        sb.AppendLine("Arguments:");
        var maxArgLength = commandDefinition.Arguments.Max(a => a.Name.Length);
        foreach (var argument in commandDefinition.Arguments.Where(a => !a.IsGlobal))
        {
            sb.Append($"\t--{argument.Name.PadRight(maxArgLength)}");
            sb.Append(argument.IsFlag ? "" : $"\t:\t<{argument.ValueType.Name}>");
            if (argument.IsRequired)
            {
                sb.Append(" [required]");
            }

            if(argument.DefaultValue is not null)
            {
                sb.Append($" (default: {argument.DefaultValue})");
            }

            sb.AppendLine();
        }

        if (commandDefinition.Arguments.Any(a => a.IsGlobal))
        {
            sb.AppendLine();
            sb.AppendLine("Global Arguments:");

            foreach (var argument in commandDefinition.Arguments.Where(a => a.IsGlobal))
            {
                sb.AppendLine($"\t--{argument.Name.PadRight(maxArgLength)}\t:\t{argument.Help}");
            }
        }
        return sb.ToString();
    }
}
