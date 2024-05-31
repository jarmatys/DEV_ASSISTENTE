using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Qdrant.Contracts;

public interface IQdrantService
{
    Task<Result> CreateCollectionAsync(string name);
    Task<Result> DropCollectionAsync(string name);
    Task<Result> UpsertAsync(DocumentDto document);
    Task<Result<List<SearchResult>>> SearchAsync(VectorDto vectorDto);
}