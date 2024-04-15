using FastEndpoints;
using FastEndpoints.Swagger;

namespace ASSISTENTE.API.Extensions.Configurations;

internal static class EndpointExtensions
{
    internal static WebApplicationBuilder AddEndpoints(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddFastEndpoints()
            .AddSwaggerDocument(options =>
            {
                options.DocumentName = "v1";
                options.Title = "Assistente API Documentation";
                options.Version = "1.0.0";
            });

        return builder;
    }
    
    internal static WebApplication UseEndpoints(this WebApplication app)
    {
        app
            .UseFastEndpoints(c =>
            {
                c.Endpoints.ShortNames = true;
            })
            .UseSwaggerGen();

        return app;
    }
}