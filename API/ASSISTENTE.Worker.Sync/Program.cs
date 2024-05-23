using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.HealthCheck;
using ASSISTENTE.Common.Logging;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.Worker.Sync.Common.Extensions;

var settings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build()
    .GetSettings<AssistenteSettings>();

var builder = WebApplication
    .CreateBuilder(args)
    .AddQueue(settings)
    .AddLogging(settings.Seq)
    .AddModules(settings)
    .AddHealthChecks(settings);

var application = builder.Build()
    .MapHealthChecks();

await application.RunAsync();