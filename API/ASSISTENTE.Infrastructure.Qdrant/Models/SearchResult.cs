using Qdrant.Client.Grpc;

namespace ASSISTENTE.Infrastructure.Qdrant.Models;

public sealed class SearchResult
{
    public IReadOnlyCollection<ScoredPoint> Value { get; set; }
}