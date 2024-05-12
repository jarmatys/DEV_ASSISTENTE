using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.Persistence.Configuration;
using ASSISTENTE.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence<TUserResolver>(
            this IServiceCollection services, 
            IConfiguration configuration)
        where TUserResolver : class, IUserResolver
        {
            services.AddConfiguration(configuration);
            
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            
            services.AddScoped<IUserResolver, TUserResolver>();
            
            return services;
        }
    }
}
