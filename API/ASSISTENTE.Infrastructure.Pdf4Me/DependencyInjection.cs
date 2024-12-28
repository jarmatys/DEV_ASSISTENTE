using ASSISTENTE.Infrastructure.Pdf4Me.Contracts;
using ASSISTENTE.Infrastructure.Pdf4Me.Settings;
using Microsoft.Extensions.DependencyInjection;
using Pdf4meClient;

namespace ASSISTENTE.Infrastructure.Pdf4Me
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddPdf4Me<TSettings>(this IServiceCollection services)
            where TSettings : IPdf4MeSettings
        {
            services.AddSingleton<Pdf4me>(serviceProvider =>
            {
                var pdf4Me = serviceProvider.GetRequiredService<TSettings>().Pdf4Me;

                var instance = Pdf4me.Instance;
                
                instance.Init(pdf4Me.PrimaryToken);
                
                return instance;
            });
            
            services.AddScoped<IPdf4MeService, Pdf4MeService>();

            return services;
        }
    }
}