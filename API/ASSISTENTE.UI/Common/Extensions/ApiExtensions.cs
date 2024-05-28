using ASSISTENTE.Common.Correlation.Consts;
using ASSISTENTE.Common.Correlation.Generators;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;

namespace ASSISTENTE.UI.Common.Extensions;

internal static class ApiExtensions
{
    public static WebAssemblyHostBuilder ConfigureApi(this WebAssemblyHostBuilder builder, ClientSettings settings)
    {
        builder.Services.AddTransient(_ => new HttpClient
        {
            BaseAddress = new Uri(settings.ApiUrl),
            DefaultRequestHeaders =
            {
                { CorrelationConsts.CorrelationHeader, CorrelationGenerator.Generate() }
            }
        });

        builder.Services.AddScoped(_ => new HubConnectionBuilder()
            .WithUrl($"{settings.HubUrl}/answers")
            .Build());
        
        return builder;
    }
}