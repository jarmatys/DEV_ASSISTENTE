using ASSISTENTE.Common.Settings;
using ASSISTENTE.Publisher.Rabbit;

namespace ASSISTENTE.API.Extensions.Configurations;

internal static class BrokerExtensions
{
    internal static WebApplicationBuilder AddMessageBroker(this WebApplicationBuilder builder, AssistenteSettings settings)
    {
        builder.Services.AddRabbitPublisher(settings.Rabbit);

        return builder;
    }
}