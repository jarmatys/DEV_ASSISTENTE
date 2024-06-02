using System.Reflection;
using ASSISTENTE.Common.Settings.Sections;
using ASSISTENTE.MessageBroker.Rabbit.Filters;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.MessageBroker.Rabbit
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPublisher(this IServiceCollection services, RabbitSection rabbit)
        {
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.UseInMemoryOutbox(ctx);

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
        
         public static IServiceCollection AddConsumers(this IServiceCollection services, Assembly assembly, RabbitSection rabbit)
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
                        cfg.UseInMemoryOutbox(ctx);
                        
                        cfg.UseConsumeFilter(typeof(ContextConsumeLoggingFilter<>), ctx);
        
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
                        
                        cfg.ReceiveEndpoint(rabbit.Name, c =>
                        {
                            foreach (var type in consumerTypes)
                            {
                                c.ConfigureConsumer(ctx, type);
                                // c.UseMessageRetry(r => r.Immediate(3));
                            }
                        });
                    });
                });
        
                return services;
            }
        
            private static List<Type> GetConsumers(Assembly assembly)
            {
                var consumerTypes = assembly.GetTypes()
                    .Where(t =>
                        t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>)) &&
                        !t.IsAbstract)
                    .ToList();
        
                return consumerTypes;
            }
    }
}