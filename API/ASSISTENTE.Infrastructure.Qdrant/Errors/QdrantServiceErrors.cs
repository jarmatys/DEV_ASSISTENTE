using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Infrastructure.Qdrant.Errors;

internal static class QdrantServiceErrors
{
    public static readonly Error UpsertFailed = new(
        "QdrantServiceErrors.UpsertFailed", "Failed to upsert embeddings");
    
    public static readonly Error SearchFailed = new(
        "QdrantServiceErrors.SearchFailed", "Failed to search embeddings");
    
    public static readonly Error ConnectionFailed = new(
        "QdrantServiceErrors.ConnectionFailed", "Failed to connect to Qdrant");
}