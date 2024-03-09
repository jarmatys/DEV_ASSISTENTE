using ASSISTENTE.Infrastructure.Embeddings.Errors;
using CSharpFunctionalExtensions;
using ASSISTENTE.Infrastructure.Embeddings.Models;
using ASSISTENTE.Infrastructure.Embeddings.ValueObjects;
using OpenAI.Net;

namespace ASSISTENTE.Infrastructure.Embeddings.Providers.OpenAI;

internal class OpenAiClient(IOpenAIService openAiService) : IEmbeddingClient
{
    public async Task<Result<EmbeddingDto>> GetAsync(EmbeddingText text)
    {
        var result = await openAiService.Embeddings.Create(text.Text);

        if (!result.IsSuccess)
            return Result.Failure<EmbeddingDto>(OpenAiClientErrors.InvalidResult.Build());

        throw new Exception();
    }
}