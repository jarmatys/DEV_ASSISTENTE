namespace ASSISTENTE.API.Extensions.Configurations;

internal static class CommonExtensions
{
    internal static WebApplicationBuilder AddCommon(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        return builder;
    }

    internal static WebApplication UseCommon(this WebApplication app)
    {
        app.UseReDoc(c =>
        {
            c.SpecUrl = "/swagger/v1/swagger.json";
            c.RoutePrefix = "redoc";
        });

        return app;
    }
}