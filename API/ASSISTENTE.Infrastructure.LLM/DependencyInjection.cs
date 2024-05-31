using ASSISTENTE.Common.Settings.Sections;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.LLM.Providers.OpenAI;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Net;

namespace ASSISTENTE.Infrastructure.LLM
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddLlm(this IServiceCollection services, OpenAiSection configuration)
        {
            services.AddScoped<ILlmClient, OpenAiClient>();
            
            services.AddOpenAIServices(options =>
            {
                options.ApiKey = configuration.ApiKey;
            });
            
            return services;
        }
    }
}
