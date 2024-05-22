using System.Reflection;
using ASSISTENTE.Common.Settings;
using MassTransit;

namespace ASSISTENTE.Worker.Sync.Common.Extensions;

internal static class QueueExtensions
{
    public static WebApplicationBuilder AddQueue(this WebApplicationBuilder builder, AssistenteSettings settings)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var consumerTypes = GetConsumers(assembly);

        builder.Services.AddMassTransit(config =>
        {
            foreach (var type in consumerTypes)
            {
                config.AddConsumer(type);
            }

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
                
                cfg.ReceiveEndpoint(settings.Rabbit.Name, c =>
                {
                    foreach (var type in consumerTypes)
                    {
                        c.ConfigureConsumer(ctx, type);
                    }
                });
            });
        });

        return builder;
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