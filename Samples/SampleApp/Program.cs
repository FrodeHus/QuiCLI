using QuiCLI;
using SampleApp;

var builder = QuicApp.CreateBuilder();
builder.Configure(config => config.CustomBanner = () => "Welcome to SampleApp!");

builder.Commands.Add<HelloCommand>()
    .WithCommand("hello", x => x.Hello)
    .WithCommand("bye", x => x.Bye);

builder.Commands.WithGroup("greetings").Add<HelloCommand>().WithCommand("sup", x => x.Hello);

var app = builder.Build();

app.Run();
