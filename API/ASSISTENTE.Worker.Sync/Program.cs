using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.HealthCheck;
using ASSISTENTE.Common.Observability;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.Worker.Sync.Common.Extensions;

var settings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build()
    .GetSettings<AssistenteSettings>();

var builder = WebApplication
    .CreateBuilder(args)
    .AddCommon(settings)
    .AddModules(settings)
    .AddHealthChecks(settings);

var application = builder.Build()
    .MapHealthChecks()
    .UseOpenTelemetry();

await application.RunAsync();