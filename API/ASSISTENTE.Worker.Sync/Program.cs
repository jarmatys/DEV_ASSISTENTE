using System.Reflection;
using ASSISTENTE.Worker.Sync.Common.Extensions;

var workerSettings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build()
    .GetSettings();

var builder = WebApplication
    .CreateBuilder(args)
    .AddQueue(workerSettings.Rabbit, Assembly.GetExecutingAssembly());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.Run();