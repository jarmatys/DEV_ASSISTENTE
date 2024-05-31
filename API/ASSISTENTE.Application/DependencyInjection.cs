using ASSISTENTE.Application.Middlewares;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly));
            services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
            services.AddMiddlewares();

            return services;
        }
    }
}