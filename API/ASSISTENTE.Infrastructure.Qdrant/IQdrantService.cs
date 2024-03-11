using ASSISTENTE.Infrastructure.Qdrant.Models;
using ASSISTENTE.Infrastructure.Qdrant.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Qdrant;

public interface IQdrantService
{
    Task<Result> CreateCollectionAsync(string name);
    
    Task<Result> UpsertAsync(Document document);
    
    Task<Result<SearchResult>> SearchAsync(Vector vector);
}