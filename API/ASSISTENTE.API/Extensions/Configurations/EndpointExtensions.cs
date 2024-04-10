using FastEndpoints;

namespace ASSISTENTE.API.Extensions.Configurations;

internal static class EndpointExtensions
{
    internal static WebApplicationBuilder AddEndpoints(this WebApplicationBuilder builder)
    {
        builder.Services.AddFastEndpoints();

        return builder;
    }
    
    internal static WebApplication UseEndpoints(this WebApplication app)
    {
        // TODO: add support for swagger: https://fast-endpoints.com/docs/swagger-support#enable-swagger

        app.UseFastEndpoints();

        return app;
    }
}