using ASSISTENTE.UI.Brokers;
using ASSISTENTE.UI.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;

namespace ASSISTENTE.UI.Common.Extensions;

internal static class ApiExtensions
{
    public static WebAssemblyHostBuilder ConfigureApi(this WebAssemblyHostBuilder builder, ClientSettings settings)
    {
        builder.Services.AddHttpClient(BrokerConst.InternalApi, settings.ApiUrl);

        builder.Services.AddScoped(_ => new HubConnectionBuilder()
            .WithUrl($"{settings.HubUrl}/answers")
            .Build());

        return builder;
    }
}