using ASSISTENTE.Common.Settings;

namespace ASSISTENTE.Client.Common;

public sealed class ClientSettings
{
    public required string ApiUrl { get; init; }
    public required SeqSection Seq { get; init; }
}