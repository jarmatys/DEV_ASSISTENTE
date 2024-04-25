using ASSISTENTE.Common.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Qdrant.Client;

namespace ASSISTENTE.Infrastructure.Qdrant
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddQdrant(this IServiceCollection services, IConfiguration configuration)
        {
            var qdrantHost = configuration["Qdrant:Host"] ?? throw new MissingSettingsException("Qdrant:Host");
            
            services.AddSingleton<QdrantClient>(_ => new QdrantClient(qdrantHost));
            services.AddScoped<IQdrantService, QdrantService>();
            
            return services;
        }
    }
}
