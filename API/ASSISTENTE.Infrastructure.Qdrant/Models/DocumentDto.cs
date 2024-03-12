using CSharpFunctionalExtensions;
using Qdrant.Client.Grpc;

namespace ASSISTENTE.Infrastructure.Qdrant.Models;

public sealed class DocumentDto : QdrantBase
{
    private DocumentDto(List<PointStruct> points, string collectionName) : base(collectionName)
    {
        Points = points;
    }

    private List<PointStruct> Points { get; }
    
    public static Result<DocumentDto> Create(IEnumerable<float> embeddings, string collectionName)
    {
        var point = new PointStruct
        {
            Id = Guid.NewGuid(),
            // Payload =
            // {
            //     ["info"] = news.Info,
            // },
            Vectors = embeddings.ToArray()
        };
        
        var points = new List<PointStruct> { point };
        
        return new DocumentDto(points, collectionName);
    }
    
    public List<PointStruct> GetPoints() => Points;
}