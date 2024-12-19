using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.LLM.OpenAi;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.LLM
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddLlm<TSettings>(this IServiceCollection services)
            where TSettings : IOpenAiSettings
        {
            services.AddOpenAi<TSettings>();

            services.AddScoped<ILlmClient, OpenAiClient>();
            
            return services;
        }
    }
}