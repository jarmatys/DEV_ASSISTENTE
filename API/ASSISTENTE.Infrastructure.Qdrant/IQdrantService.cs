using ASSISTENTE.Infrastructure.Qdrant.Models;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Qdrant;

public interface IQdrantService
{
    Task<Result> CreateCollectionAsync(string name);
    
    Task<Result> UpsertAsync(DocumentDto document);
    
    Task<Result<SearchResult>> SearchAsync(VectorDto vectorDto);
}