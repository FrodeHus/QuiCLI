# QuiCLI

[![.NET](https://github.com/FrodeHus/QuiCLI/actions/workflows/dotnet.yml/badge.svg)](https://github.com/FrodeHus/QuiCLI/actions/workflows/dotnet.yml)
[![Publish](https://github.com/FrodeHus/QuiCLI/actions/workflows/nuget.yml/badge.svg)](https://github.com/FrodeHus/QuiCLI/actions/workflows/nuget.yml)
[![CodeQL](https://github.com/FrodeHus/QuiCLI/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/FrodeHus/QuiCLI/actions/workflows/github-code-scanning/codeql)

A lightweight framework for creating CLI applications with the primary focus of being compatible with AOT and trimming. This is to generate small and fast self-contained executables.

Because of the restrictions imposed by trimming and AOT, the framework is designed to be as simple as possible. This means that it does not support certain features that are common in other CLI frameworks that requires extensive reflection.

The framework is built on top of Microsoft.Extensions.DependencyInjection and Microsoft.Extensions.Configuration, which means that it is possible to use the same configuration and dependency injection system as in ASP.NET Core.

As an example of an application that uses QuiCLI, see [SecurityCenterCLI](https://github.com/FrodeHus/SecurityCenterCLI) and [RFDump](https://github.com/FrodeHus/RFDump).

## Features

- Dependency injection
- Command line argument parsing
- Command line argument help generation
- Command line argument type conversion
- Nested command groups

## Quick start

_Note:_ The framework is still in early development, and the API is subject to change.

The only supported returns types for asynchronous commands are `Task<object>`, `Task<string>` and `Task` due to reflection restrictions. For synchronous commands, everything is supported.

Registration of commands is explicit by design to make the code as efficient and clear as possible without heavy reflection usage.

`dotnet add package QuiCLI`

```csharp

using Microsoft.Extensions.DependencyInjection;
using QuiCLI;
using SampleApp;

var builder = QuicApp.CreateBuilder();
builder.Configure(config => config.CustomBanner = () => "Welcome to SampleApp!");
builder.Services.AddTransient<GreeterService>();

builder.Commands.Add<HelloCommand>()
    .WithCommand("hello", x => x.Hello)
    .WithCommand("bye", x => x.Bye);

var informalGroup = builder.Commands.WithGroup("informal");

informalGroup
    .Add<InformalGreetings>()
    .WithCommand("sup", x => x.Sup)
    .WithCommand("hey", x => x.Hey);

informalGroup
    .Add<InformalGoodbyes>()
    .WithCommand("cya", x => x.Cya)
    .WithCommand("later", x => x.Later);

var app = builder.Build();

app.Run();



```

```csharp
using QuiCLI.Command;

namespace SampleApp;

internal class HelloCommand(GreeterService greeterService)
{
    private readonly GreeterService _greeterService = greeterService;

    public string Hello(
        [Parameter(help: "Which name to greet")] string name,
        [Parameter(help: "Define which year should be displayed")] int year = 2024)
    {
        return $"Hello, {name}! Welcome to {year}!";
    }

    public string Bye(string name)
    {
        return _greeterService.SayGoodbye(name);
    }
}
```

```cmd
myapp hello --name "World"
```

```cmd
sampleapp.exe --help

Welcome to SampleApp!
Usage:
  <command> [arguments]

Commands:
        hello
        bye

Nested Commands:
        informal

Global Arguments:
        --help          :       Show help information
        --output        :       Output format
```
