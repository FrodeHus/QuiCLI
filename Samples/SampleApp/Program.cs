using QuiCLI;
using SampleApp;

var builder = QuicApp.CreateBuilder();
builder.Configure(config => config.CustomBanner = () => "Welcome to SampleApp!");

builder.Commands.AddCommand<HelloCommand>("hello")
    .UseMethod(x => x.Hello);

var app = builder.Build();

app.AddCommandGroup("sub-group").AddCommand(_ => new HelloCommand());
app.Run();
