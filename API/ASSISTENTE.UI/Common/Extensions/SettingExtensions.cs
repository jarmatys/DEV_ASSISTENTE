using ASSISTENTE.UI.Common.Exceptions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ASSISTENTE.UI.Common.Extensions;

public static class SettingExtensions
{
    public static ClientSettings GetSettings(this WebAssemblyHostConfiguration webAssemblyHostBuilder)
    {
        return webAssemblyHostBuilder.GetSection("Settings").Get<ClientSettings>() 
               ?? throw new ClientException("Missing client settings.");
    }
}