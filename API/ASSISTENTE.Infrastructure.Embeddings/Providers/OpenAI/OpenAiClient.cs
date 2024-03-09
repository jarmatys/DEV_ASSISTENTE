using CSharpFunctionalExtensions;
using ASSISTENTE.Infrastructure.Embeddings.Models;
using ASSISTENTE.Infrastructure.Embeddings.ValueObjects;

namespace ASSISTENTE.Infrastructure.Embeddings.Providers.OpenAI;

internal class OpenAiClient : IEmbeddingClient
{
    public async Task<Result<EmbeddingDto>> GetAsync(EmbeddingText text)
    {
        throw new NotImplementedException();
    }
}