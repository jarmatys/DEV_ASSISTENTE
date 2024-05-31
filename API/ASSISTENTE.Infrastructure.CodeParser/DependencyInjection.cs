using ASSISTENTE.Infrastructure.CodeParser.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.CodeParser
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddCodeParser(this IServiceCollection services)
        {
            services.AddScoped<ICodeParser, CodeParser>();
            
            return services;
        }
    }
}
