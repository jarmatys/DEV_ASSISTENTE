using ASSISTENTE.Common.HealthCheck;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.LLM.HealthChecks;
using ASSISTENTE.Infrastructure.LLM.Providers.OpenAI;
using ASSISTENTE.Infrastructure.LLM.Settings;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Net;

namespace ASSISTENTE.Infrastructure.LLM
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddLlm<TSettings>(this IServiceCollection services)
            where TSettings : ILlmSettings
        {
            var llmSettings = services.BuildServiceProvider().GetRequiredService<TSettings>().Llm;
            
            services.AddScoped<ILlmClient, OpenAiClient>();
            
            services.AddOpenAIServices(options =>
            {
                options.ApiKey = llmSettings.ApiKey;
            });

            services.AddCommonHealthCheck<LlmHealthCheck>();
            
            return services;
        }
    }
}
