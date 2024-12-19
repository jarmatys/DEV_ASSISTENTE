using ASSISTENTE.Infrastructure.Embeddings.Contracts;
using ASSISTENTE.Infrastructure.LLM.OpenAi;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.Embeddings
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddEmbeddings<TSettings>(this IServiceCollection services)
            where TSettings : IOpenAiSettings
        {
            services.AddOpenAi<TSettings>();
            
            services.AddScoped<IEmbeddingClient, OpenAiClient>();
            
            return services;
        }
    }
}