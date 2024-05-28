using ASSISTENTE.Common.Settings.Sections;
using ASSISTENTE.Publisher.Rabbit.Filters;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Publisher.Rabbit
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRabbitPublisher(this IServiceCollection services, RabbitSection rabbit)
        {
            // TODO: Move here publisher configuration and consumers registration
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.UsePublishFilter(typeof(ContextPublishLoggingFilter<>), ctx);
                    
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