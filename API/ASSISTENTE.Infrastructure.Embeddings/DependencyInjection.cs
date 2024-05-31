using ASSISTENTE.Common.Settings.Sections;
using ASSISTENTE.Infrastructure.Embeddings.Contracts;
using ASSISTENTE.Infrastructure.Embeddings.Providers.OpenAI;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Net;

namespace ASSISTENTE.Infrastructure.Embeddings
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddEmbeddings(this IServiceCollection services, OpenAiSection configuration)
        {
            services.AddScoped<IEmbeddingClient, OpenAiClient>();
            
            services.AddOpenAIServices(options =>
            {
                options.ApiKey = configuration.ApiKey;
            });
            
            return services;
        }
    }
}
