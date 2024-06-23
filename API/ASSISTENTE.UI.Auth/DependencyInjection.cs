using ASSISTENTE.UI.Auth.Common.Extensions;
using ASSISTENTE.UI.Auth.Common.Settings;
using ASSISTENTE.UI.Common;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ASSISTENTE.UI.Auth
{
    public static class DependencyInjection
    {
        public static WebAssemblyHostBuilder AddAuthenticationModule(
            this WebAssemblyHostBuilder builder, 
            AuthenticationSettings settings)
        {
            builder.AddCommonModule();
            
            builder.AddAuthentication(settings);
            
            return builder;
        }
    }
}