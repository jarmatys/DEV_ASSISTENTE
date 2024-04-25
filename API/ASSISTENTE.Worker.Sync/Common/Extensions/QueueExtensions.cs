using System.Reflection;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.Common.Settings.Sections;
using MassTransit;

namespace ASSISTENTE.Worker.Sync.Common.Extensions;

internal static class QueueExtensions
{
    public static WebApplicationBuilder AddQueue(
        this WebApplicationBuilder builder, 
        RabbitSection rabbitSettings, 
        Assembly assembly)
    {
        var consumerTypes = GetConsumers(assembly);

        builder.Services.AddMassTransit(config =>
        {
            foreach (var type in consumerTypes)
            {
                config.AddConsumer(type);
            }

            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(rabbitSettings.Url);
                cfg.ReceiveEndpoint(rabbitSettings.Name, c =>
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