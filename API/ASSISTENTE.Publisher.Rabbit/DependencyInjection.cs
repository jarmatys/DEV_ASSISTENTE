using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Common.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Publisher.Rabbit
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRabbitPublisher(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSettings<AssistenteSettings>();
            
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(settings.Rabbit.Url, h =>
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