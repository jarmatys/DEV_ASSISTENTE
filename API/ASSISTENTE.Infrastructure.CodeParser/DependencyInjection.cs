using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.CodeParser
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCodeParser(this IServiceCollection services)
        {
            services.AddScoped<ICodeParser, CodeParser>();
            
            return services;
        }
    }
}
