using ASSISTENTE.Common.Settings.Sections;

namespace ASSISTENTE.UI.Common;

public sealed class ClientSettings
{
    public required string Version { get; init; }
    public required string ApiUrl { get; init; }
    public required string HubUrl { get; init; }
    public required SeqSection Seq { get; init; }
}