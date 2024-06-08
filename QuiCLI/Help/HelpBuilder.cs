using QuiCLI.Command;
using System.Text;

namespace QuiCLI.Help;

internal class HelpBuilder(CommandGroup rootCommandGroup)
{
    private readonly CommandGroup rootCommandGroup = rootCommandGroup;

    public string BuildHelp()
    {
        var sb = new StringBuilder();
        var groupName = rootCommandGroup.Name ?? "";
        sb.AppendLine("Usage:");
        sb.AppendLine($" {groupName} <command> [arguments]");
        sb.AppendLine();
        var commandSection = new HelpSection("Commands");
        foreach (var command in rootCommandGroup.Commands)
        {
            commandSection.Items.Add(new HelpCommand(command
                .Key
                .Name, command.Key.Help, command
                    .Key
                    .Arguments
                    .ConvertAll(a => new HelpArgument(a.Name, a.IsRequired, a.IsFlag, a.Help))
));
        }
        sb.AppendLine(commandSection.ToString());
        sb.AppendLine();
        if (rootCommandGroup.SubGroups.Count > 0)
        {
            var nestedSection = new HelpSection("Nested Commands");
            foreach (var group in rootCommandGroup.SubGroups)
            {
                sb.AppendLine($"\t{group.Key}");
            }
        }
        if (rootCommandGroup.GlobalArguments.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("Global Arguments:");
            foreach (var argument in rootCommandGroup.GlobalArguments)
            {
                sb.AppendLine($"\t--{argument.Name}\t:\t{argument.Help}");
            }
        }
        return sb.ToString();
    }

    public string BuildHelp(CommandDefinition commandDefinition)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Usage: {commandDefinition.Name}");
        if (!string.IsNullOrWhiteSpace(commandDefinition.Help))
        {
            sb.AppendLine(commandDefinition.Help);
        }
        sb.AppendLine();
        sb.AppendLine("Arguments:");
        foreach (var argument in commandDefinition.Arguments.Where(a => !a.IsGlobal))
        {
            sb.Append($"\t--{argument.Name}");
            sb.Append(argument.IsFlag ? "" : $"\t:\t<{argument.ValueType.Name}>");
            if (argument.IsRequired)
            {
                sb.Append(" [required]");
            }
            sb.AppendLine();
        }

        if (commandDefinition.Arguments.Any(a => a.IsGlobal))
        {
            sb.AppendLine();
            sb.AppendLine("Global Arguments:");
            foreach (var argument in commandDefinition.Arguments.Where(a => a.IsGlobal))
            {
                sb.AppendLine($"\t--{argument.Name}\t:\t{argument.Help}");
            }
        }
        return sb.ToString();
    }

    internal static IHelpItem GenerateHelp(CommandDefinition commandDefinition)
    {
        return new HelpCommand(commandDefinition.Name,
                               commandDefinition.Help,
                               commandDefinition.Arguments.ConvertAll(a => new HelpArgument(a.Name, a.IsRequired, a.IsFlag, a.Help)));
    }
}
