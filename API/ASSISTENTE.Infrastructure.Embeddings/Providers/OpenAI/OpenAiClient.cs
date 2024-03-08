using ASSISTENTE.Infrastructure.Embeddings.Models;

namespace ASSISTENTE.Infrastructure.Embeddings.Providers.OpenAI;

internal class OpenAiClient : IEmbeddingClient
{
    public async Task<EmbeddingDto> GetAsync(string text)
    {
        throw new NotImplementedException();
    }
}