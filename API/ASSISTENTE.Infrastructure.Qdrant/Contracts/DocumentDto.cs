using CSharpFunctionalExtensions;
using Qdrant.Client.Grpc;

namespace ASSISTENTE.Infrastructure.Qdrant.Contracts;

public sealed class DocumentDto : QdrantBase
{
    private DocumentDto(string collectionName, List<PointStruct> points) : base(collectionName)
    {
        Points = points;
    }

    private List<PointStruct> Points { get; }

    public static Result<DocumentDto> Create(
        string collectionName,
        IEnumerable<float> embeddings,
        Guid? resourceId = null)
    {
        var point = new PointStruct
        {
            Id = resourceId ?? Guid.NewGuid(),
            Vectors = embeddings.ToArray()
        };

        var points = new List<PointStruct> { point };

        return new DocumentDto(collectionName, points);
    }

    public static Result<DocumentDto> Create(
        string collectionName,
        IEnumerable<float> embeddings,
        Dictionary<string, string>? metadata = null,
        Guid? resourceId = null)
    {
        var point = new PointStruct
        {
            Id = resourceId ?? Guid.NewGuid(),
            Vectors = embeddings.ToArray()
        };

        if (metadata != null)
        {
            foreach (var (key, value) in metadata)
            {
                point.Payload.Add(key, new Value { StringValue = value });
            }
        }

        var points = new List<PointStruct> { point };

        return new DocumentDto(collectionName, points);
    }

    public List<PointStruct> GetPoints() => Points;
}