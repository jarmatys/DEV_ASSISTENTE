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
        
        if (!response.IsSuccess)
            return Result.Failure<EmbeddingDto>(OpenAiClientErrors.InvalidResult.Build(response.ErrorResponse?.Error.Message!));

        var embeddings = response.Result?.Data.First().Embedding.ToList();
        
        return embeddings is null
            ? Result.Failure<EmbeddingDto>(OpenAiClientErrors.EmptyEmbeddings.Build())
            : Result.Success(EmbeddingDto.Create(embeddings));
    }
}