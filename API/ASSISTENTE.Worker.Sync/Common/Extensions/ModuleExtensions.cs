using ASSISTENTE.Module;
using ASSISTENTE.Worker.Sync.Services;

namespace ASSISTENTE.Worker.Sync.Common.Extensions;

internal static class ModuleExtensions
{
    public static WebApplicationBuilder AddModules(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddAssistenteModule<UserResolver>(configuration);
    
        return builder;
    }

}