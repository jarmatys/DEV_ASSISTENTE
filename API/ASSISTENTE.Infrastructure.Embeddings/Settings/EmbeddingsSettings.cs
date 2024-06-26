namespace ASSISTENTE.Infrastructure.Embeddings.Settings;

public sealed class EmbeddingsSettings
{
    public required string ApiKey { get; init; }
    public required string OrganizationId { get; init; }
    public required string ProjectId { get; init; }
}