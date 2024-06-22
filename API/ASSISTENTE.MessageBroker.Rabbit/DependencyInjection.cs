using System.Reflection;
using ASSISTENTE.Common.HealthCheck;
using ASSISTENTE.MessageBroker.Rabbit.Filters;
using ASSISTENTE.MessageBroker.Rabbit.HealthChecks;
using ASSISTENTE.MessageBroker.Rabbit.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.MessageBroker.Rabbit
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommonPublisher<TSettings>(this IServiceCollection services)
            where TSettings : IRabbitSettings
        {
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    var publisherSettings = ctx.GetRequiredService<TSettings>().Rabbit;

                    cfg.UseInMemoryOutbox(ctx);

                    cfg.UsePublishFilter(typeof(ContextPublishLoggingFilter<>), ctx);

                    cfg.Host(publisherSettings.Url, h =>
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

        public static IServiceCollection AddCommonConsumers<TSettings>(this IServiceCollection services, Assembly assembly)
            where TSettings : IRabbitSettings
        {
            var consumerTypes = GetConsumers(assembly);

            services.AddMassTransit(config =>
            {
                foreach (var type in consumerTypes)
                {
                    config.AddConsumer(type);
                }

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    var consumerSettings = ctx.GetRequiredService<TSettings>().Rabbit;
                    
                    cfg.UseInMemoryOutbox(ctx);

                    cfg.UseConsumeFilter(typeof(ContextConsumeLoggingFilter<>), ctx);

                    cfg.Host(consumerSettings.Url, h =>
                    {
                        h.ConfigureBatchPublish(bcfg =>
                        {
                            bcfg.Enabled = true;
                            bcfg.MessageLimit = 100;
                            bcfg.SizeLimit = 10000;
                            bcfg.Timeout = TimeSpan.FromMilliseconds(30);
                        });
                    });

                    cfg.ReceiveEndpoint(consumerSettings.Name, c =>
                    {
                        foreach (var type in consumerTypes)
                        {
                            c.ConfigureConsumer(ctx, type);
                        }
                    });
                });
            });

            services.AddCommonHealthCheck<MessageBrokerHealthCheck>();

            return services;
        }

        private static List<Type> GetConsumers(Assembly assembly)
        {
            var consumerTypes = assembly.GetTypes()
                .Where(t =>
                    t.GetInterfaces().Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>)) &&
                    !t.IsAbstract)
                .ToList();

            return consumerTypes;
        }
    }
}