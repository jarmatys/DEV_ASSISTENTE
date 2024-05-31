using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Embeddings.Contracts;

public interface IEmbeddingClient
{
    Task<Result<EmbeddingDto>> GetAsync(EmbeddingText text);
}