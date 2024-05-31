using ASSISTENTE.Common.Settings.Sections;
using ASSISTENTE.Infrastructure.Qdrant.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Qdrant.Client;

namespace ASSISTENTE.Infrastructure.Qdrant
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddQdrant(this IServiceCollection services, QdrantSection configuration)
        {
            services.AddSingleton<QdrantClient>(_ => new QdrantClient(
                host: configuration.Host, 
                port: configuration.ClientPort)
            );
            
            services.AddScoped<IQdrantService, QdrantService>();
            
            return services;
        }
    }
}
