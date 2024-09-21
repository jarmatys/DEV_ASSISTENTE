using ASSISTENTE.API;
using ASSISTENTE.API.Common.Extensions;
using SOFTURE.Common.HealthCheck;
using SOFTURE.Common.Observability;
using SOFTURE.Settings.Extensions;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build()
    .ValidateSettings<ApiSettings>();

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureSettings<ApiSettings>(configuration)
    .AddCommon<ApiSettings>()
    .AddEndpoints()
    .AddCors()
    .AddLimiter()
    .AddHubs()
    .AddModules<ApiSettings>();

var application = builder.Build()
    .UseRedoc()
    .UseCors()
    .UseLimiter()
    .UseEndpoints();

application
    .MapHubs()
    .MapCommonHealthChecks()
    .UseCommonOpenTelemetry()
    .UseMiddlewares();

await application.RunAsync();