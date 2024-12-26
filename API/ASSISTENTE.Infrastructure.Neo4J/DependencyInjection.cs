using ASSISTENTE.Infrastructure.Neo4J.Contracts;
using ASSISTENTE.Infrastructure.Neo4J.Settings;
using Microsoft.Extensions.DependencyInjection;
using Neo4j.Driver;

namespace ASSISTENTE.Infrastructure.Neo4J
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddNeo4J<TSettings>(this IServiceCollection services)
            where TSettings : INeo4JSettings
        {
            services.AddSingleton<IDriver>(serviceProvider =>
            {
                var neo4JSettings = serviceProvider.GetRequiredService<TSettings>().Neo4J;

                return GraphDatabase.Driver(
                    neo4JSettings.Url,
                    AuthTokens.Basic(neo4JSettings.User, neo4JSettings.Password)
                );
            });
            
            services.AddScoped<INeo4JService, Neo4JService>();

            return services;
        }
    }
}