using ASSISTENTE.API.Common.Services;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.Module;

namespace ASSISTENTE.API.Common.Extensions;

internal static class ModuleExtensions
{
    internal static WebApplicationBuilder AddModules(this WebApplicationBuilder builder, AssistenteSettings settings)
    {
        builder.Services.AddAssistenteModule<UserResolver>(settings);

        return builder;
    }
}