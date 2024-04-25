using ASSISTENTE.Common.Exceptions;
using ASSISTENTE.Infrastructure.Embeddings.Providers.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Net;

namespace ASSISTENTE.Infrastructure.Embeddings
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmbeddings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmbeddingClient, OpenAiClient>();
            
            var openAiApiKey = configuration["OpenAI:ApiKey"] ?? throw new MissingSettingsException("OpenAI:ApiKey");
            
            services.AddOpenAIServices(options =>
            {
                options.ApiKey = openAiApiKey;
            });
            
            return services;
        }
    }
}
