namespace ASSISTENTE.API.Extensions.Configurations;

internal static class CorsConst
{
    internal const string AllowAll = "AllowAll";
}

internal static class CorsExtensions
{
    internal static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: CorsConst.AllowAll,
                policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
        });

        return builder;
    }
    
    internal static WebApplication UseCors(this WebApplication app)
    {
        app.UseCors(CorsConst.AllowAll);

        return app;
    }
}