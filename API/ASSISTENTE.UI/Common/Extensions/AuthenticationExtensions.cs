using ASSISTENTE.UI.Common.Settings;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Supabase;

namespace ASSISTENTE.UI.Common.Extensions;

internal class SubabaseClient(AuthenticationSettings settings, SupabaseOptions options)
    : Client(settings.Url, settings.PublicKey, options);


internal static class AuthenticationExtensions
{
    public static WebAssemblyHostBuilder AddAuthentication(this WebAssemblyHostBuilder builder,
        AuthenticationSettings settings)
    {
        var supaBaseOptions = new SupabaseOptions
        {
            AutoConnectRealtime = true,
            AutoRefreshToken = true
        };

        var supabaseClient = new SubabaseClient(settings, supaBaseOptions);
        
        builder.Services.AddScoped<SubabaseClient>(_ => supabaseClient);
        
        return builder;
    }
}