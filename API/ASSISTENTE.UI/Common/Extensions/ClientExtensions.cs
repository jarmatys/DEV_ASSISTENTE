using ASSISTENTE.UI.Brokers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ASSISTENTE.UI.Common.Extensions;

internal static class ClientExtensions
{
    public static WebAssemblyHostBuilder ConfigureClient(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddBrokers();
        
        return builder;
    }
}