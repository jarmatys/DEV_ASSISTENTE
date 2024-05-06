using ASSISTENTE.Application.Middlewares.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Application.Middlewares
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMiddlewares(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidationBehaviour<,>));

            return services;
        }
    }
}
