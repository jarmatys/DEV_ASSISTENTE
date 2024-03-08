using ASSISTENTE.Infrastructure.Embeddings.Providers.OpenAI;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.Embeddings
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmbeddings(this IServiceCollection services)
        {
            services.AddScoped<IEmbeddingClient, OpenAiClient>();

            return services;
        }
    }
}
