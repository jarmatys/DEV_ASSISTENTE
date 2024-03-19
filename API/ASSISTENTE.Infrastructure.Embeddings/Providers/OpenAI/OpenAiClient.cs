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
        var response = await openAiService.Embeddings.Create(text.Text);
        
        // TODO: measure token window and handle limit
        // This model's maximum context length is 8192 tokens, however you requested 8829 tokens (8829 in your prompt; 0 for the completion).
        // Please reduce your prompt; or completion length.

        if (!response.IsSuccess)
            return Result.Failure<EmbeddingDto>(OpenAiClientErrors.InvalidResult.Build(response.ErrorResponse?.Error.Message!));

        var embeddings = response.Result?.Data.First().Embedding.Select(i => (float)i);
        
        return embeddings is null
            ? Result.Failure<EmbeddingDto>(OpenAiClientErrors.EmptyEmbeddings.Build())
            : Result.Success(EmbeddingDto.Create(embeddings));
    }
}