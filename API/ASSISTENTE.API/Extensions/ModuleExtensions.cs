using ASSISTENTE.API.Services;
using ASSISTENTE.Module;

namespace ASSISTENTE.API.Extensions;

internal static class ModuleExtensions
{
    internal static WebApplicationBuilder AddModules(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddAssistenteModule<UserResolver>(configuration);

        return builder;
    }
}