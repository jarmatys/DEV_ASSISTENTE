using ASSISTENTE.Common.Settings.Sections;

namespace ASSISTENTE.Common.Settings;

public sealed class AssistenteSettings
{
    public required RabbitSection Rabbit { get; set; }
    public required InternalApiSection InternalApi { get; set; }
}