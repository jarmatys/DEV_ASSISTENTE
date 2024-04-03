using ASSISTENTE.Infrastructure.Embeddings.Errors;
using CSharpFunctionalExtensions;
using ASSISTENTE.Infrastructure.Embeddings.Models;
using ASSISTENTE.Infrastructure.Embeddings.ValueObjects;
using OpenAI.Net;
using SharpToken;

namespace ASSISTENTE.Infrastructure.Embeddings.Providers.OpenAI;

internal class OpenAiClient(IOpenAIService openAiService) : IEmbeddingClient
{
    private const string EmbeddingModel = "text-embedding-ada-002";
    private const int MaxTokens = 8192;

    public async Task<Result<EmbeddingDto>> GetAsync(EmbeddingText text)
    {
        return await TextCanBeProcessable(text)
            .Bind(async () =>
            {
                var response = await openAiService.Embeddings.Create(text.Text);

                if (!response.IsSuccess)
                    return Result.Failure<EmbeddingDto>(
                        OpenAiClientErrors.InvalidResult.Build(response.ErrorResponse?.Error.Message!));

                var embeddings = response.Result?.Data.First().Embedding.Select(i => (float)i);

                return embeddings is null
                    ? Result.Failure<EmbeddingDto>(OpenAiClientErrors.EmptyEmbeddings.Build())
                    : Result.Success(EmbeddingDto.Create(embeddings));
            });
    }

    private static Result TextCanBeProcessable(EmbeddingText text)
    {
        var encoding = GptEncoding.GetEncodingForModel(EmbeddingModel);
        var tokens = encoding.CountTokens(text.Text);

        if (tokens > MaxTokens)
        {
            Result.Failure<EmbeddingDto>(OpenAiClientErrors.TooManyTokens.Build());
        }

        return Result.Success();
    }
}