using ASSISTENTE.Common.Settings.Sections;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.EventHandlers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEvents(this IServiceCollection services, RabbitSection rabbitSection)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));
            
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbitSection.Url, h =>
                    {
                        h.ConfigureBatchPublish(bcfg =>
                        {
                            bcfg.Enabled = true;
                            bcfg.MessageLimit = 100;
                            bcfg.SizeLimit = 10000;
                            bcfg.Timeout = TimeSpan.FromMilliseconds(30);
                        });
                    });
                });
            });
            
            return services;
        }
    }
}