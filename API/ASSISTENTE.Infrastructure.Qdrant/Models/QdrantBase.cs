namespace ASSISTENTE.Infrastructure.Qdrant.Models;

public abstract class QdrantBase(string collectionName)
{
    private string CollectionName { get; } = collectionName;
    
    public string GetCollectionName() => CollectionName;
}