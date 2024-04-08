namespace ASSISTENTE.API.Extensions;

internal static class CorsConst
{
    internal const string AllowAll = "AllowAll";
}

internal static class CorsExtensions
{
    internal static void AddCors(this WebApplicationBuilder builder)
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
    }
    
    internal static void UseCors(this WebApplication app)
    {
        app.UseCors(CorsConst.AllowAll);
    }
}