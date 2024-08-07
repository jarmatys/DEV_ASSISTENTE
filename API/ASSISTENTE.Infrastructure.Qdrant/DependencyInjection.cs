using ASSISTENTE.Infrastructure.Qdrant.Contracts;
using ASSISTENTE.Infrastructure.Qdrant.HealthChecks;
using ASSISTENTE.Infrastructure.Qdrant.Settings;
using Microsoft.Extensions.DependencyInjection;
using Qdrant.Client;
using SOFTURE.Common.HealthCheck;

namespace ASSISTENTE.Infrastructure.Qdrant
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddQdrant<TSettings>(this IServiceCollection services)
            where TSettings : IQdrantSettings
        {
            services.AddSingleton<QdrantClient>(serviceProvider =>
            {
                var qdrantSettings = serviceProvider.GetRequiredService<TSettings>().Qdrant;

                return new QdrantClient(
                    host: qdrantSettings.Host,
                    port: qdrantSettings.ClientPort);
            });

            services.AddScoped<IQdrantService, QdrantService>();

            services.AddCommonHealthCheck<QdrantHealthCheck>();
            
            return services;
        }
    }
}