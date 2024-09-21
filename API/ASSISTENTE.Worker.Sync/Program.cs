using ASSISTENTE.Worker.Sync;
using ASSISTENTE.Worker.Sync.Common.Extensions;
using SOFTURE.Common.HealthCheck;
using SOFTURE.Common.Observability;
using SOFTURE.Settings.Extensions;

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