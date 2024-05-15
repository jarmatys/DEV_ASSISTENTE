using ASSISTENTE.Common.Settings.Sections;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Publisher.Rabbit
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRabbitPublisher(this IServiceCollection services, RabbitSection rabbit)
        {
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbit.Url, h =>
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