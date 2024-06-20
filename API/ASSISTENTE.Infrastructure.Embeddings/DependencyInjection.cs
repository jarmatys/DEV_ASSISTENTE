using ASSISTENTE.Infrastructure.Embeddings.Contracts;
using ASSISTENTE.Infrastructure.Embeddings.Providers.OpenAI;
using ASSISTENTE.Infrastructure.Embeddings.Settings;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Net;

namespace ASSISTENTE.Infrastructure.Embeddings
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddEmbeddings<TSettings>(this IServiceCollection services)
            where TSettings : IEmbeddingsSettings
        {
            var embeddingsSettings = services.BuildServiceProvider().GetRequiredService<TSettings>().Embeddings;

            services.AddScoped<IEmbeddingClient, OpenAiClient>();
            
            services.AddOpenAIServices(options =>
            {
                options.ApiKey = embeddingsSettings.ApiKey;
            });
            
            return services;
        }
    }
}
