using ASSISTENTE.Client.Internal.Settings;
using ASSISTENTE.Infrastructure;
using ASSISTENTE.Persistence.Configuration.Settings;

namespace ASSISTENTE.Module;

public interface IModuleSettings : 
    IInternalApiSettings, 
    IDatabaseSettings, 
    IInfrastructureInterface
{
}