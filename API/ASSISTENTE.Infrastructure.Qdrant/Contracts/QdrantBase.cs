namespace ASSISTENTE.Infrastructure.Qdrant.Contracts;

public abstract class QdrantBase(string collectionName)
{
    private string CollectionName { get; } = collectionName;
    
    public string GetCollectionName() => CollectionName;
}