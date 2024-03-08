using ASSISTENTE.Infrastructure.Embeddings.Models;

namespace ASSISTENTE.Infrastructure.Embeddings;

public interface IEmbeddingClient
{
    Task<EmbeddingDto> GetAsync(string text);
}