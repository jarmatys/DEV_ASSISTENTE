using CSharpFunctionalExtensions;using ASSISTENTE.Infrastructure.Embeddings.Models;
using ASSISTENTE.Infrastructure.Embeddings.ValueObjects;

namespace ASSISTENTE.Infrastructure.Embeddings;

public interface IEmbeddingClient
{
    Task<Result<EmbeddingDto>> GetAsync(EmbeddingText text);
}