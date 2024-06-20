namespace ASSISTENTE.Infrastructure.Qdrant.Settings;

public sealed class QdrantSettings
{
    public required string Host { get; init; }
    public required int ClientPort { get; init; }
    public required int ApiPort { get; init; }
}