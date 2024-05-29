namespace QuiCLI.Command;

internal sealed class CommandLineParser
{
    public CommandLineParser(CommandGroup rootCommandGroup)
    {
        _rootCommandGroup = rootCommandGroup;
    }

    private const string LongOptionPrefix = "--";
    private const string ShortOptionPrefix = "-";
    private readonly CommandGroup _rootCommandGroup;
    private CommandGroup? _currentCommandGroup;

    public (ParsedCommand?, CommandGroup) Parse(string[] args)
    {
        _currentCommandGroup = _rootCommandGroup;
        if (args == null) return (null, _rootCommandGroup);

        args = args.Where(o => !string.IsNullOrWhiteSpace(o)).ToArray();
        ParsedCommand? command = null;
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (string.IsNullOrWhiteSpace(arg)) continue;

            if (command is not null && IsArgument(arg))
            {
                var option = GetArgumentName(arg);
                object? value;
                if (!TryGetArgumentValue(arg, out var optionValue) && i + 1 < args.Length
                        && !IsNextArgument(args, i))
                {
                    value = args[i + 1];
                    i++;
                }
                else
                {
                    value = optionValue;
                }

                value = EnsureValueType(command.Name, option, value ?? string.Empty);
                command.AddArgument(option, value);
            }
            else if (TryGetCommandDefinition(arg, out var definition))
            {
                command = new ParsedCommand(arg, definition!, _currentCommandGroup!);
            }
            else if (TryGetCommandGroup(arg, out var group))
            {
                _currentCommandGroup = group;
            }
        }
        return (command, _currentCommandGroup ?? _rootCommandGroup);
    }

    private object EnsureValueType(string commandName, string parameterName, object value)
    {
        if (_rootCommandGroup is null)
        {
            return value;
        }

        var (definition, _) = _rootCommandGroup.GetCommand(commandName);
        if (definition == null) return value;
        var argumentDefinition = definition.GetParameter(parameterName);
        if (argumentDefinition == null) return value;
        if (argumentDefinition.IsFlag)
        {
            return true;
        }
        return Convert.ChangeType(value, argumentDefinition.ValueType);

    }

    private static bool IsArgument(string arg)
    {
        return IsLongArgument(arg) || IsShortArgument(arg);
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

    private static bool IsShortArgument(string arg)
    {
        return arg.StartsWith(ShortOptionPrefix);
    }

    private static bool IsLongArgument(string arg)
    {
        return arg.StartsWith(LongOptionPrefix);
    }

    private static string GetArgumentName(string arg)
    {
        if (IsLongArgument(arg))
        {
            return arg[2..];
        }
        else if (IsShortArgument(arg))
        {
            return arg[1..];
        }
        return arg;
    }

    private static bool TryGetArgumentValue(string arg, out string? value)
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

    private static bool IsNextArgument(string[] args, int index)
    {
        return LookAhead(args, index) && IsArgument(args[index + 1]);
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
