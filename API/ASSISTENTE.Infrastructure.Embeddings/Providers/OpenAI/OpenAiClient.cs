using ASSISTENTE.Infrastructure.Embeddings.Contracts;
using ASSISTENTE.Infrastructure.Embeddings.Errors;
using CSharpFunctionalExtensions;
using OpenAI;
using SharpToken;

namespace ASSISTENTE.Infrastructure.Embeddings.Providers.OpenAI;

internal class OpenAiClient(OpenAIClient client) : IEmbeddingClient
{
    private const string EmbeddingModel = "text-embedding-ada-002";
    private const int MaxTokens = 8192;

    public async Task<Result<EmbeddingDto>> GetAsync(EmbeddingText text)
    {
        return await TextCanBeProcessable(text)
            .Bind(async () =>
            {
                var response = await client.EmbeddingsEndpoint.CreateEmbeddingAsync(
                    input: text.Text,
                    model: EmbeddingModel
                );

                var embeddings = response.Data.Select(x => x.Embedding).FirstOrDefault()?.Select(x => (float)x);

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