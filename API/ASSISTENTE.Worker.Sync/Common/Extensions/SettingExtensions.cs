using ASSISTENTE.Worker.Sync.Common.Exceptions;

namespace ASSISTENTE.Worker.Sync.Common.Extensions;

public static class SettingExtensions
{
    public static WorkerSettings GetSettings(this IConfiguration configuration)
    {
        return configuration.Get<WorkerSettings>() 
               ?? throw new WorkerException("Missing worker settings.");
    }
}