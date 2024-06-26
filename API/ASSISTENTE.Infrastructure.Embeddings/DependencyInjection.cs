using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Infrastructure.Embeddings.Contracts;
using ASSISTENTE.Infrastructure.Embeddings.Providers.OpenAI;
using ASSISTENTE.Infrastructure.Embeddings.Settings;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;

namespace ASSISTENTE.Infrastructure.Embeddings
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddEmbeddings<TSettings>(this IServiceCollection services)
            where TSettings : IEmbeddingsSettings
        {
            var settings = services.GetSettings<TSettings, EmbeddingsSettings>(x => x.Embeddings);

            services.AddScoped<IEmbeddingClient, OpenAiClient>();

            services.AddScoped<OpenAIClient>(_ => 
                new OpenAIClient(new OpenAIAuthentication(settings.ApiKey), client: new HttpClient()));
            
            return services;
        }
    }
}