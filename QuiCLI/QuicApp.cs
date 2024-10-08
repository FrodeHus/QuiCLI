﻿using QuiCLI.Builder;
using QuiCLI.Command;
using QuiCLI.Help;
using QuiCLI.Middleware;

namespace QuiCLI;

public class QuicApp
{
    public required Configuration Configuration { get; init; }
    public required IServiceProvider ServiceProvider { get; init; }
    public required QuicMiddlewareDelegate Pipeline { get; init; }
    public required CommandGroup RootCommands { get; init; }
    internal List<ParameterDefinition> GlobalArguments { get; init; } = [];
    public static QuicAppBuilder CreateBuilder()
    {
        return new QuicAppBuilder();
    }


    public int Run(string? commandLine = null)
    {
        return RunAsync(commandLine).GetAwaiter().GetResult();
    }
    public async Task<int> RunAsync(string? commandLine = null)
    {

        var parser = new CommandLineParser(RootCommands, Configuration);
        var result = parser.Parse(commandLine is not null
            ? commandLine.Split(' ')
            : Environment.GetCommandLineArgs().Skip(1).ToArray());
        if (result.IsFailure)
        {
            Console.WriteLine(result.Error);
            var helpBuilder = new HelpBuilder(result.Value.CommandGroup ?? RootCommands, Configuration);
            Console.WriteLine(helpBuilder.BuildHelp());
            Environment.ExitCode = 1;
        }
        var command = result.Value.ParsedCommand;
        if (command is not null)
        {
            var globalArguments = command.Arguments.Where(a => a.Argument.IsGlobal).ToList();
            var context = new QuicCommandContext(command, Configuration) { ServiceProvider = ServiceProvider, GlobalArguments = globalArguments };
            var executionResult = await Pipeline(context);
            if (executionResult == 0)
            {
                Console.WriteLine(context.CommandResult ?? string.Empty);
            }

            Environment.ExitCode = executionResult;
        }
        else
        {
            var helpBuilder = new HelpBuilder(result.Value.CommandGroup!, Configuration);
            Console.WriteLine(helpBuilder.BuildHelp());
        }
        return Environment.ExitCode;
    }
}
