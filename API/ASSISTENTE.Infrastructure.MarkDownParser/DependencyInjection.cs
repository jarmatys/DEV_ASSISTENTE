using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.MarkDownParser
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMarkDownParser(this IServiceCollection services)
        {
            services.AddScoped<IMarkDownParser, MarkDownParser>();
            
            return services;
        }
    }
}
