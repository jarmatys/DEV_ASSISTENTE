using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Infrastructure.Qdrant.Errors;

internal static class QdrantServiceErrors
{
    public static readonly Error UpsertFailed = new(
        "QdrantServiceErrors.UpsertFailed", "Failed to upsert embeddings");
}