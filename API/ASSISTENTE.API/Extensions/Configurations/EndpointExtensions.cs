using FastEndpoints;
using FastEndpoints.Swagger;

namespace ASSISTENTE.API.Extensions.Configurations;

internal static class EndpointExtensions
{
    internal static WebApplicationBuilder AddEndpoints(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddFastEndpoints()
            .SwaggerDocument(options =>
            {
                options.ShortSchemaNames = true;
                options.DocumentSettings = settings =>
                {
                    settings.DocumentName =  "v1";
                    settings.Title = "Assistente API Documentation";
                    settings.Version = "1.0.0";
                };
            });

        return builder;
    }

    internal static WebApplication UseEndpoints(this WebApplication app)
    {
        app
            .UseFastEndpoints(c =>
            {
                c.Endpoints.RoutePrefix = "api";
                c.Endpoints.ShortNames = true;

                c.Endpoints.Configurator = endpointDefinition =>
                {
                    endpointDefinition.AllowAnonymous();
                };
            })
            .UseSwaggerGen();

        return app;
    }
}