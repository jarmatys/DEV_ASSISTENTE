namespace ASSISTENTE.Infrastructure.Qdrant.ValueObjects;

public sealed class Vector(string collectionName) : QdrantBase(collectionName)
{
    public List<float> Value { get; set; }
}
