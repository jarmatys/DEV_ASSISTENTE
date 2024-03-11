namespace ASSISTENTE.Infrastructure.Qdrant.ValueObjects;

public abstract class QdrantBase(string collectionName)
{
    protected string CollectionName { get; } = collectionName;
}