using Google.Protobuf.Collections;
using Qdrant.Client.Grpc;

namespace ASSISTENTE.Infrastructure.Qdrant.Contracts;

public sealed class SearchResult
{
    private SearchResult(Guid resourceId, float score, Dictionary<string, string> metadata)
    {
        ResourceId = resourceId;
        Score = score;
        Metadata = metadata;
    }
 
    public Guid ResourceId { get; }
    public float Score { get; }
    public Dictionary<string, string> Metadata { get; }

    public static SearchResult Create(PointId pointId, float score, MapField<string, Value> payload)
    {
        var resourceId = Guid.Parse(pointId.Uuid);
        var metadata = payload.ToDictionary(x => x.Key, x => x.Value.StringValue);
        
        return new SearchResult(resourceId, score, metadata);
    }
}