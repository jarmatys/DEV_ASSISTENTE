using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace ASSISTENTE.Common.HealthCheck
{
    public static class DependencyInjection
    {
        public static WebApplication MapHealthChecks(this WebApplication app)
        {
            app.MapHealthChecks("/hc", new HealthCheckOptions
            {
                ResponseWriter = Writer.WriteResponse,
                Predicate = healthCheck => healthCheck.Tags.Contains(Consts.HealthCheckTag)
            });

            return app;
        }
    }
}