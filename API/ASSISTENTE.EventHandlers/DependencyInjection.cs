using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.EventHandlers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEvents(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));
            
            return services;
        }
    }
}