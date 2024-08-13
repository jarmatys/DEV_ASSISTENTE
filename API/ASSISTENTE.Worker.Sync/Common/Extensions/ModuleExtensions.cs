using ASSISTENTE.Module;
using ASSISTENTE.Worker.Sync.Services;
using Microsoft.AspNetCore.Builder;

namespace ASSISTENTE.Worker.Sync.Common.Extensions;

internal static class ModuleExtensions
{
    public static WebApplicationBuilder AddModules<TSettings>(this WebApplicationBuilder builder)
        where TSettings : IModuleSettings
    {
        builder.Services.AddAssistenteModule<UserResolver, TSettings>();
    
        return builder;
    }
}