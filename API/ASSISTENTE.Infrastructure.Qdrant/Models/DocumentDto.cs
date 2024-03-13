using CSharpFunctionalExtensions;
using Qdrant.Client.Grpc;

namespace ASSISTENTE.Infrastructure.Qdrant.Models;

public sealed class DocumentDto : QdrantBase
{
    private DocumentDto(string collectionName, List<PointStruct> points) : base(collectionName)
    {
        Points = points;
    }

    private List<PointStruct> Points { get; }
    
    public static Result<DocumentDto> Create(string collectionName, IEnumerable<float> embeddings)
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
        
        return new DocumentDto(collectionName, points);
    }
    
    public List<PointStruct> GetPoints() => Points;
}