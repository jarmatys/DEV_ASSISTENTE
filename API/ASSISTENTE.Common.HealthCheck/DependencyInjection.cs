using System.Reflection;
using ASSISTENTE.Common.HealthCheck.Core;
using ASSISTENTE.Common.HealthCheck.Presentation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Common.HealthCheck
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHealthCheck<THealthCheck>(this IServiceCollection services)
            where THealthCheck : CheckBase, ICommonHealthCheck
        {
            var healthCheckname = typeof(THealthCheck).Name.Replace("HealthCheck", string.Empty);
            
            services.AddHealthChecks()
                .AddCheck<THealthCheck>(healthCheckname, tags: new[] { Consts.HealthCheckTag });
            
            return services;
        }

        public static WebApplication MapHealthChecks(this WebApplication app)
        {
            app.MapHealthChecks("/hc", new HealthCheckOptions
            {
                ResponseWriter = Writer.WriteResponse,
                Predicate = healthCheck => healthCheck.Tags.Contains(Consts.HealthCheckTag)
            });

            return app;
        }

        private static List<Type> GetCommonHealthCheckTypes()
        {
            var referencedAssemblies = Assembly.GetEntryAssembly()!.GetReferencedAssemblies();
            var healthCheckInterface = typeof(ICommonHealthCheck);

            var allHealthChecks = new List<Type>();
            
            foreach (var assembly in referencedAssemblies)
            {
                var loadedAssembly = Assembly.Load(assembly);
                
                var healthChecks = loadedAssembly!.GetTypes()
                    .Where(t => healthCheckInterface.IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
                    .ToList();
                
                allHealthChecks.AddRange(healthChecks);
            }

            return allHealthChecks;
        }
    }
}