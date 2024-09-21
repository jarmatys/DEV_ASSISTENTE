using ASSISTENTE.Persistence.Configuration.HealthChecks;
using ASSISTENTE.Persistence.Configuration.Interceptors;
using ASSISTENTE.Persistence.Configuration.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SOFTURE.Common.HealthCheck;
using SOFTURE.Settings.Extensions;

namespace ASSISTENTE.Persistence.Configuration
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddConfiguration<TSettings>(this IServiceCollection services)
            where TSettings : IDatabaseSettings
        {
            services.AddScoped<EventInterceptor>();

            var settings = services.GetSettings<TSettings, DatabaseSettings>(x => x.Database);
            
            services.AddScoped<IAssistenteDbContext, AssistenteDbContext>();
            
            services.AddDbContext<AssistenteDbContext>((serviceProvider, options) => options.UseNpgsql(
                    settings.ConnectionString
                )
                .AddInterceptors(serviceProvider.GetRequiredService<EventInterceptor>())
            );
            
            services.AddCommonHealthCheck<DatabaseHealthCheck>();

            return services;
        }
    }
}
