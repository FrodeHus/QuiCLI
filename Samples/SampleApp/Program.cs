using QuiCLI;
using SampleApp;

var builder = QuicApp.CreateBuilder();
builder.Configure(config => config.CustomBanner = () => "Welcome to SampleApp!");

builder.Commands.Add<HelloCommand>()
    .WithCommand("hello", x => x.Hello)
    .WithCommand("bye", x => x.Bye);

var app = builder.Build();

app.AddCommandGroup("sub-group").AddCommand(_ => new HelloCommand());
app.Run();
