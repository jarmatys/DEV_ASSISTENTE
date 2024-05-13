using ASSISTENTE.Common.Exceptions;
using ASSISTENTE.Common.Settings.Sections;
using ASSISTENTE.Infrastructure.Embeddings.Providers.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Net;

namespace ASSISTENTE.Infrastructure.Embeddings
{
    public static class DependencyInjection
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
