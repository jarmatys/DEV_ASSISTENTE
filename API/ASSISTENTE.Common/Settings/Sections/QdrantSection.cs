namespace ASSISTENTE.Common.Settings.Sections;

public sealed class QdrantSection
{
    public required string Host { get; init; }
    public required int Port { get; init; }
}