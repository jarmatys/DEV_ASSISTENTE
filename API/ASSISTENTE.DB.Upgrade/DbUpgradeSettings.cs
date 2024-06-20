using ASSISTENTE.Persistence.Configuration.Settings;

namespace ASSISTENTE.DB.Upgrade;

internal sealed class DbUpgradeSettings : IDatabaseSettings
{
    public required DatabaseSettings Database { get; init; }
}