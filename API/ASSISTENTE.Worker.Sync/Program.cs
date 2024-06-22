using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.HealthCheck;
using ASSISTENTE.Common.Observability;
using ASSISTENTE.Worker.Sync;
using ASSISTENTE.Worker.Sync.Common.Extensions;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build()
    .ValidateSettings<WorkerSettings>();

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureSettings<WorkerSettings>(configuration)
    .AddCommon<WorkerSettings>()
    .AddModules<WorkerSettings>();

var application = builder.Build()
    .MapCommonHealthChecks()
    .UseCommonOpenTelemetry();

await application.RunAsync();