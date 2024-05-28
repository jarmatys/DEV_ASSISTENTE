using ASSISTENTE.API.Hubs;

namespace ASSISTENTE.API.Common.Extensions;

internal static class HubExtensions
{
    internal static WebApplicationBuilder AddHubs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR();

        return builder;
    }

    internal static WebApplication MapHubs(this WebApplication app)
    {
        app.MapHub<QuestionHub>("answers");

        return app;
    }
}