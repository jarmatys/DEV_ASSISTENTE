using ASSISTENTE.UI.Common.Settings;

namespace ASSISTENTE.UI.Common;

public sealed class ClientSettings
{
    public required string Version { get; init; }
    public required string ApiUrl { get; init; }
    public required string HubUrl { get; init; }
    public required SeqSettings Seq { get; init; }
    public required AuthenticationSettings Authentication { get; init; }
}