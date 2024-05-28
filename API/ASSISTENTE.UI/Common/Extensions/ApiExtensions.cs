using ASSISTENTE.UI.Common.Middlewares;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;

namespace ASSISTENTE.UI.Common.Extensions;

internal static class ApiExtensions
{
    public const string InternalApi = "internal-api";
    
    public static WebAssemblyHostBuilder ConfigureApi(this WebAssemblyHostBuilder builder, ClientSettings settings)
    {
        builder.Services.AddScoped<RequestContextLoggingMiddleware>();

        builder.Services
            .AddHttpClient(InternalApi, client => { client.BaseAddress = new Uri(settings.ApiUrl); })
            .AddHttpMessageHandler<RequestContextLoggingMiddleware>();

        builder.Services.AddScoped(_ => new HubConnectionBuilder()
            .WithUrl($"{settings.HubUrl}/answers")
            .Build());

        return builder;
    }
}