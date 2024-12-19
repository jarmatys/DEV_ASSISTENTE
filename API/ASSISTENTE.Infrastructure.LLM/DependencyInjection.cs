using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.LLM.Ollama;
using ASSISTENTE.Infrastructure.LLM.Ollama.Settings;
using ASSISTENTE.Infrastructure.LLM.OpenAi;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.LLM
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddOpenAiLlm<TSettings>(this IServiceCollection services)
            where TSettings : IOpenAiSettings
        {
            services.AddOpenAi<TSettings>();

            services.AddScoped<ILlmClient, OpenAiClient>();
            
            return services;
        }
        
        public static IServiceCollection AddOllamaLlm<TSettings>(this IServiceCollection services)
            where TSettings : IOllamaSettings
        {
            services.AddOllama<TSettings>();
        
            services.AddScoped<ILlmClient, OllamaClient>();
            
            return services;
        }
    }
}