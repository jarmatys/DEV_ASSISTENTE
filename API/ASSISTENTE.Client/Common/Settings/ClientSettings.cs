namespace ASSISTENTE.Client.Common.Settings;

public sealed class ClientSettings
{
    public required string ApiUrl { get; init; }
    public required SeqSection Seq { get; init; }
}