using ASSISTENTE.Infrastructure.MarkDownParser.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.MarkDownParser
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddMarkDownParser(this IServiceCollection services)
        {
            services.AddScoped<IMarkDownParser, MarkDownParser>();
            
            return services;
        }
    }
}
