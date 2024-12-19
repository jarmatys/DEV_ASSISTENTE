using ASSISTENTE.Infrastructure.LLM.OpenAi.HealthChecks;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Settings;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using SOFTURE.Common.HealthCheck;
using SOFTURE.Settings.Extensions;

namespace ASSISTENTE.Infrastructure.LLM.OpenAi
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddOpenAi<TSettings>(this IServiceCollection services)
            where TSettings : IOpenAiSettings
        {
            var settings = services.GetSettings<TSettings, OpenAiSettings>(x => x.OpenAi);

            services.AddScoped<OpenAIClient>(_ =>
                new OpenAIClient(
                    new OpenAIAuthentication(
                        apiKey: settings.ApiKey,
                        organization: settings.OrganizationId,
                        projectId: settings.ProjectId)
                )
            );

            services.AddCommonHealthCheck<OpenAiHealthCheck>();

            return services;
        }
    }
}