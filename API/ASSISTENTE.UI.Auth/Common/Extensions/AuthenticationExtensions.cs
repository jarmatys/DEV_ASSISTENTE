using ASSISTENTE.UI.Auth.Common.Settings;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Supabase;

namespace ASSISTENTE.UI.Auth.Common.Extensions;

public class SubabaseClient(AuthenticationSettings settings, SupabaseOptions options)
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
        
        builder.Services.AddSingleton<SubabaseClient>(_ => supabaseClient);
        
        return builder;
    }
}