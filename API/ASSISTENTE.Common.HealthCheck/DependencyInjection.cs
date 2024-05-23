using ASSISTENTE.Common.HealthCheck.Checks;
using ASSISTENTE.Common.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Common.HealthCheck
{
    public static class DependencyInjection
    {
        public static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder, AssistenteSettings settings)
        {
            builder.Services.AddSingleton<AssistenteSettings>(_ => settings);
            
            builder.Services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("Database", tags: [Consts.HealthCheckTag])
                .AddCheck<MessageBrokerHealthCheck>("MessageBroker", tags: [Consts.HealthCheckTag])
                .AddCheck<LlmHealthCheck>("LLM", tags: [Consts.HealthCheckTag])
                .AddCheck<SeqHealthCheck>("Seq", tags: [Consts.HealthCheckTag])
                .AddCheck<QdrantHealthCheck>("Qdrant", tags: [Consts.HealthCheckTag])
                .AddCheck<InternalApiHealthCheck>("InternalAPI", tags: [Consts.HealthCheckTag]);

            return builder;
        }
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