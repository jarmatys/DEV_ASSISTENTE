using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.Persistence.Configuration;
using ASSISTENTE.Persistence.Configuration.Settings;
using ASSISTENTE.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence<TUserResolver, TSettings>(this IServiceCollection services)
            where TUserResolver : class, IUserResolver
            where TSettings : IDatabaseSettings
        {
            services.AddConfiguration<TSettings>();

            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            services.AddScoped<IUserResolver, TUserResolver>();

            return services;
        }
    }
}