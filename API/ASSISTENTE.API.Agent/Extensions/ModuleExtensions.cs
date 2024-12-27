using ASSISTENTE.Module;

namespace ASSISTENTE.API.Agent.Extensions;

internal static class ModuleExtensions
{
    internal static WebApplicationBuilder AddModules<TSettings>(this WebApplicationBuilder builder)
        where TSettings : IModuleSettings
    {
        builder.Services.AddAssistenteModule<UserResolver, TSettings>();

        return builder;
    }
}