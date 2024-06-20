using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Client.Internal.HealthChecks;
using ASSISTENTE.Client.Internal.Settings;
using ASSISTENTE.Common.HealthCheck;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Client.Internal
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInternalClient<TSettings>(this IServiceCollection services)
            where TSettings : IInternalApiSettings
        {
            services.AddScoped(serviceProvider => new HttpClient
            {
                BaseAddress = new Uri(serviceProvider.GetRequiredService<TSettings>().InternalApi.Url)
            });
            
            services.AddScoped<IAssistenteClientInternal, AssistenteClientInternal>();

            services.AddHealthCheck<InternalApiHealthCheck>();
            
            return services;
        }
    }
}