namespace ASSISTENTE.API.Common.Extensions;

internal static class RedocExtensions
{
    internal static WebApplication UseRedoc(this WebApplication app)
    {
        app.UseReDoc(c =>
        {
            c.SpecUrl = "/swagger/v1/swagger.json";
            c.RoutePrefix = "redoc";
        });

        return app;
    }
}