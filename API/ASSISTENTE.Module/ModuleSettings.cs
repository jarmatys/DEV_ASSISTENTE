using ASSISTENTE.Common.Settings.Sections;

namespace ASSISTENTE.Module;

public sealed class ModuleSettings
{
    public required RabbitSection Rabbit { get; init; }
}