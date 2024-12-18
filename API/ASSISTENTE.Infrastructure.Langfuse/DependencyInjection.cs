using ASSISTENTE.Infrastructure.Langfuse.Contracts;
using ASSISTENTE.Infrastructure.Langfuse.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure.Langfuse
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddLangfuse<TSettings>(this IServiceCollection services)
            where TSettings : ILangfuseSettings
        {
            // services.AddSingleton<[CLIENT]>(serviceProvider =>
            // {
            //     var langfuseSettings = serviceProvider.GetRequiredService<TSettings>().Langfuse;
            //
            //     return [CLIENT]
            // });

            services.AddScoped<ILangfuseService, LangfuseService>();

            return services;
        }
    }
}