using ASSISTENTE.Common.Settings;
using ASSISTENTE.Publisher.Rabbit;

namespace ASSISTENTE.API.Common.Extensions;

internal static class MessageBrokerExtensions
{
    internal static WebApplicationBuilder AddMessageBroker(this WebApplicationBuilder builder, AssistenteSettings settings)
    {
        builder.Services.AddRabbitPublisher(settings.Rabbit);

        return builder;
    }
}