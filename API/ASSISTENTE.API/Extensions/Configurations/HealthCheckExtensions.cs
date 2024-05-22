using ASSISTENTE.Common.HealthCheck;
using ASSISTENTE.Persistence.Configuration;

namespace ASSISTENTE.API.Extensions.Configurations;

public static class HealthCheckExtensions
{
    internal static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database", tags: [Consts.HealthCheckTag]);

        return builder;
    }
}