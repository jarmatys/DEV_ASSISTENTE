using ASSISTENTE.Common.Settings;
using ASSISTENTE.Module;
using ASSISTENTE.Worker.Sync.Services;

namespace ASSISTENTE.Worker.Sync.Common.Extensions;

internal static class ModuleExtensions
{
    public static WebApplicationBuilder AddModules(this WebApplicationBuilder builder, AssistenteSettings settings)
    {
        builder.Services.AddAssistenteModule<UserResolver>(settings);
    
        return builder;
    }

}