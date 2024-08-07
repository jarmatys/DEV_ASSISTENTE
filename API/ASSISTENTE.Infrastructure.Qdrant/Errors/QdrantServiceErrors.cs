using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.Qdrant.Errors;

internal static class QdrantServiceErrors
{
    public static readonly Error UpsertFailed = new(
        "QdrantServiceErrors.UpsertFailed", "Failed to upsert embeddings");
    
    public static readonly Error MissingResources = new(
        "QdrantServiceErrors.MissingResources", "Missing resources attached to provided context - please add new resources.");
    
    public static readonly Error ConnectionFailed = new(
        "QdrantServiceErrors.ConnectionFailed", "Failed to connect to Qdrant");
}