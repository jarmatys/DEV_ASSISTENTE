namespace ASSISTENTE.API.Extensions;

internal static class CommonExtensions
{
    internal static WebApplicationBuilder AddCommon(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }
    
    internal static WebApplication UseCommon(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        return app;
    }
}