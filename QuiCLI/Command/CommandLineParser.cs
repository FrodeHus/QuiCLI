namespace QuiCLI.Command;

internal sealed class CommandLineParser
{
    public CommandLineParser()
    {
    }

    public CommandLineParser(CommandGroup rootCommandGroup)
    {
        _rootCommandGroup = rootCommandGroup;
    }

    private const string LongOptionPrefix = "--";
    private const string ShortOptionPrefix = "-";
    private readonly CommandGroup? _rootCommandGroup;
    private CommandGroup? _currentCommandGroup;

    public IList<ParsedCommand> Parse(string[] args)
    {
        _currentCommandGroup = _rootCommandGroup;
        if (args == null) return [];
        args = args.Where(o => !string.IsNullOrWhiteSpace(o)).ToArray();
        var commands = new List<ParsedCommand>();
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (string.IsNullOrWhiteSpace(arg)) continue;

            if (IsOption(arg))
            {
                var command = commands[^1];
                var option = GetOptionName(arg);
                object? value;
                if (!TryGetOptionValue(arg, out var optionValue) && i + 1 < args.Length
                        && !IsNextOption(args, i))
                {
                    value = args[i + 1];
                    i++;
                }
                else
                {
                    value = optionValue;
                }

                value = EnsureValueType(command.Name, option, value ?? string.Empty);
                commands[^1].AddOption(option, value);
            }
            else if (TryGetCommandDefinition(arg, out var definition))
            {
                commands.Add(new ParsedCommand(arg, definition));
            }
            else if (TryGetCommandGroup(arg, out var group))
            {
                _currentCommandGroup = group;
            }
        }
        return commands;
    }

    private object EnsureValueType(string commandName, string parameterName, object value)
    {
        if (_rootCommandGroup is null)
        {
            return value;
        }

        var (definition, _) = _rootCommandGroup.GetCommand(commandName);
        if (definition == null) return value;
        var optionDefinition = definition.GetParameter(parameterName);
        if (optionDefinition == null) return value;
        if (optionDefinition.IsFlag)
        {
            return true;
        }
        return Convert.ChangeType(value, optionDefinition.ValueType);

    }

    private static bool IsOption(string arg)
    {
        return IsLongOption(arg) || IsShortOption(arg);
    }

    private bool TryGetCommandGroup(string groupName, out CommandGroup? group)
    {
        if (_currentCommandGroup == null)
        {
            group = null;
            return false;
        }

        group = (_currentCommandGroup.Name == groupName) ? _currentCommandGroup : _currentCommandGroup.SubGroups.FirstOrDefault(g => g.Value.Name == groupName).Value;
        return group != null;
    }

    private static bool IsShortOption(string arg)
    {
        return arg.StartsWith(ShortOptionPrefix);
    }

    private static bool IsLongOption(string arg)
    {
        return arg.StartsWith(LongOptionPrefix);
    }

    private static string GetOptionName(string arg)
    {
        if (IsLongOption(arg))
        {
            return arg[2..];
        }
        else if (IsShortOption(arg))
        {
            return arg[1..];
        }
        return arg;
    }

    private static bool TryGetOptionValue(string arg, out string? value)
    {
        if (!arg.Contains('='))
        {
            value = arg;
            return false;
        }

        var parts = arg.Split("=", StringSplitOptions.RemoveEmptyEntries);
        value = parts[1].Trim();

        return true;
    }

    private static bool LookAhead(string[] args, int index) => index + 1 < args.Length;

    private static bool IsNextOption(string[] args, int index)
    {
        return LookAhead(args, index) && IsOption(args[index + 1]);
    }

    private bool TryGetCommandDefinition(string name, out CommandDefinition? definition)
    {
        if (_currentCommandGroup == null)
        {
            definition = null;
            return false;
        }

        (definition, _) = _currentCommandGroup.GetCommand(name);
        return definition != null;
    }
}
