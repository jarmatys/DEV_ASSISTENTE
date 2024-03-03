using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.FileParser
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFileParser(this IServiceCollection services)
        {
            services.AddScoped<IFileParser, FileParser>();
            
            return services;
        }
    }
}
