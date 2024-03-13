using Qdrant.Client.Grpc;

namespace ASSISTENTE.Infrastructure.Qdrant.Models;

public sealed class SearchResult
{
    private SearchResult(Guid resourceId, float score)
    {
        ResourceId = resourceId;
        Score = score;
    }
    public Guid ResourceId { get; }
    public float Score { get; }

    public static SearchResult Create(PointId pointId, float score)
    {
        var resourceId = Guid.Parse(pointId.Uuid);
        
        return new SearchResult(resourceId, score);
    }
}