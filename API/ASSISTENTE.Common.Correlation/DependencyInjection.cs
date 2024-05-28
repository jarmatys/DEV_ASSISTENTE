using ASSISTENTE.Common.Correlation.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Common.Correlation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCorrelationProvider(this IServiceCollection services)
        {
            services.AddScoped<ICorrelationProvider, CorrelationProvider>();
            
            return services;
        }
    }
}