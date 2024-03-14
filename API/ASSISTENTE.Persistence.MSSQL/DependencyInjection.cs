using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Persistence.MSSQL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMssql(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AssistenteDatabase");
            
            services.AddDbContext<AssistenteDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IAssistenteDbContext, AssistenteDbContext>();

            return services;
        }
    }
}
