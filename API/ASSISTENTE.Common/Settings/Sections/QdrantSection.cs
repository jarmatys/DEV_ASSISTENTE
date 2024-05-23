namespace ASSISTENTE.Common.Settings.Sections;

public sealed class QdrantSection
{
    public required string Host { get; init; }
    public required int ClientPort { get; init; }
    public required int ApiPort { get; init; }
}