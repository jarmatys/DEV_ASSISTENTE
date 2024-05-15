using ASSISTENTE.API.Services;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.Module;

namespace ASSISTENTE.API.Extensions.Configurations;

internal static class ModuleExtensions
{
    internal static WebApplicationBuilder AddModules(this WebApplicationBuilder builder, AssistenteSettings settings)
    {
        builder.Services.AddAssistenteModule<UserResolver>(settings);

        return builder;
    }
}