using QuiCLI;
using SampleApp;

var builder = QuicApp.CreateBuilder();
builder.Configure(config =>
{
    config.CustomBanner = () => "Welcome to SampleApp!";
});
var app = builder.Build();

app.AddCommand(_ => new HelloCommand());
app.AddCommandGroup("sub-group").AddCommand(_ => new HelloCommand());
app.Run();
