using ASSISTENTE.UI.Auth.Common.Extensions;
using ASSISTENTE.UI.Auth.Common.Settings;
using ASSISTENTE.UI.Auth.Providers;
using ASSISTENTE.UI.Common;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.UI.Auth
{
    public static class DependencyInjection
    {
        public static WebAssemblyHostBuilder AddAuthenticationModule(
            this WebAssemblyHostBuilder builder, 
            AuthenticationSettings settings)
        {
            builder.Services.AddScoped<AuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<AuthStateProvider>());
            builder.Services.AddAuthorizationCore();

            builder.Services.AddBlazoredLocalStorage(config =>
            {
                config.JsonSerializerOptions.WriteIndented = true;
            });

            builder.AddCommonModule();
            
            builder.AddAuthentication(settings);
            
            return builder;
        }
    }
}