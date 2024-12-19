using ASSISTENTE.Infrastructure.Embeddings.Contracts;
using ASSISTENTE.Infrastructure.LLM.Ollama;
using ASSISTENTE.Infrastructure.LLM.Ollama.Settings;
using ASSISTENTE.Infrastructure.LLM.OpenAi;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.Embeddings
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddOpenAiEmbeddings<TSettings>(this IServiceCollection services)
            where TSettings : IOpenAiSettings
        {
            services.AddOpenAi<TSettings>();
            
            services.AddScoped<IEmbeddingClient, OpenAiClient>();
            
            return services;
        }
        
        public static IServiceCollection AddOllamaEmbeddings<TSettings>(this IServiceCollection services)
            where TSettings : IOllamaSettings
        {
            services.AddOllama<TSettings>();
            
            services.AddScoped<IEmbeddingClient, OllamaClient>();
            
            return services;
        }
    }
}