using System.Reflection;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Worker.Sync.Common;
using ASSISTENTE.Worker.Sync.Common.Extensions;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var workerSettings = configuration.GetSettings<WorkerSettings>();

var builder = WebApplication
    .CreateBuilder(args)
    .AddQueue(workerSettings.Rabbit, Assembly.GetExecutingAssembly())
    .AddLogging(configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.Run();