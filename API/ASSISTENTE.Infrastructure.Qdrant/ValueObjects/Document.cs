using Qdrant.Client.Grpc;

namespace ASSISTENTE.Infrastructure.Qdrant.ValueObjects;

public sealed class Document(string collectionName) : QdrantBase(collectionName)
{
    public List<PointStruct> Points { get; set; }
}