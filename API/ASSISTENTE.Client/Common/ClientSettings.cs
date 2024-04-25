using ASSISTENTE.Common.Settings;
using ASSISTENTE.Common.Settings.Sections;

namespace ASSISTENTE.Client.Common;

public sealed class ClientSettings
{
    public required string ApiUrl { get; init; }
    public required SeqSection Seq { get; init; }
}