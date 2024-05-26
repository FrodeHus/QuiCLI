namespace QuiCLI.Command;

internal sealed class CommandLineParser
{
    public CommandLineParser()
    {
    }

    public CommandLineParser(CommandDataSource commands)
    {
        _commands = commands;
    }

    private const string LongOptionPrefix = "--";
    private const string ShortOptionPrefix = "-";
    private readonly CommandDataSource? _commands;

    public IList<ParsedCommand> Parse(string[] args)
    {
        if (args == null) return [];
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
            else
            {
                if (IsDefinedCommand(arg))
                {
                    commands.Add(new ParsedCommand(arg));
                }
            }
        }
        return commands;
    }

    private object EnsureValueType(string commandName, string optionName, object value)
    {
        var definition = _commands?.GetCommand(commandName);
        if (definition == null) return value;
        var optionDefinition = definition.GetOption(optionName);
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

    private bool IsDefinedCommand(string name)
    {
        if (_commands == null) return true;
        return _commands?.GetCommand(name) != null;
    }
}
