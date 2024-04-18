using ASSISTENTE.Client.Common.Exceptions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ASSISTENTE.Client.Common.Extensions;

public static class SettingExtensions
{
    public static ClientSettings GetSettings(this WebAssemblyHostConfiguration webAssemblyHostBuilder)
    {
        return webAssemblyHostBuilder.GetSection("Settings").Get<ClientSettings>() 
               ?? throw new ClientException("Missing client settings.");
    }
}