using ASSISTENTE.Infrastructure.CodeParser;
using ASSISTENTE.Infrastructure.FileParser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFileParser();
            services.AddCodeParser();
            
            return services;
        }
    }
}
