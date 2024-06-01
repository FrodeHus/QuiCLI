# QuiCLI

[![.NET](https://github.com/FrodeHus/QuiCLI/actions/workflows/dotnet.yml/badge.svg)](https://github.com/FrodeHus/QuiCLI/actions/workflows/dotnet.yml)
[![Publish](https://github.com/FrodeHus/QuiCLI/actions/workflows/nuget.yml/badge.svg)](https://github.com/FrodeHus/QuiCLI/actions/workflows/nuget.yml)
[![CodeQL](https://github.com/FrodeHus/QuiCLI/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/FrodeHus/QuiCLI/actions/workflows/github-code-scanning/codeql)

A lightweight framework for creating CLI applications with the primary focus of being compatible with AOT and trimming. This is to generate small and fast self-contained executables.

Because of the restrictions imposed by trimming and AOT, the framework is designed to be as simple as possible. This means that it does not support certain features that are common in other CLI frameworks that requires extensive reflection.

The framework is built on top of Microsoft.Extensions.DependencyInjection and Microsoft.Extensions.Configuration, which means that it is possible to use the same configuration and dependency injection system as in ASP.NET Core.

As an example of an application that uses QuiCLI, see [SecurityCenterCLI](https://github.com/FrodeHus/SecurityCenterCLI).

## Features

- Dependency injection
- Command line argument parsing
- Command line argument help generation
- Command line argument type conversion
- Command line argument default values

## Quick start

_Note:_ The framework is still in early development, and the API is subject to change.

The only supported returns types for asynchronous commands are `Task<object>` and `Task` due to reflection restrictions.

```csharp

using QuiCLI;

var builder = QuicApp.CreateBuilder();
builder.Services.AddTransient<MyService>();
var app = builder.Build();

app.AddCommand((sp) => new HelloCommand(sp.GetRequiredService<MyService>()));
app.Run();
```

```csharp
public class HelloCommand(MyService myService)
{
	private readonly MyService _service = myService;

	[Command("hello")]
	public string Hello(string name)
	{
		return $"Hello, {name}!";
	})
}
```

```cmd
myapp hello --name "World"
```
