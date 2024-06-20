using ASSISTENTE.API;
using ASSISTENTE.API.Common.Extensions;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.HealthCheck;
using ASSISTENTE.Common.Observability;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
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
    .MapHealthChecks()
    .UseOpenTelemetry()
    .UseMiddlewares();

await application.RunAsync();