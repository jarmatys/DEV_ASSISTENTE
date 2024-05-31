using ASSISTENTE.Infrastructure.Embeddings.Contracts;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Embeddings.Providers.OpenSource;

internal class OpenSourceClient : IEmbeddingClient
{
    // For the data privacy reasons, in the future open source embeddings will be added
    // Example service: https://github.com/clems4ever/torchserve-all-minilm-l6-v2
    // More information: https://www.youtube.com/watch?v=QdDoFfkVkcw
    
    public Task<Result<EmbeddingDto>> GetAsync(EmbeddingText text)
    {
        throw new NotImplementedException();
    }
}