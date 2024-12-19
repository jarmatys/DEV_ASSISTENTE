using ASSISTENTE.Infrastructure.Embeddings.Contracts;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Errors;
using CSharpFunctionalExtensions;
using OllamaSharp;
using SharpToken;

namespace ASSISTENTE.Infrastructure.Embeddings;

internal class OllamaClient(OllamaApiClient client) : IEmbeddingClient
{
    private const string EmbeddingModel = "<MODEL>";
    private const int MaxTokens = 8192;

    public async Task<Result<EmbeddingDto>> GetAsync(EmbeddingText text)
    {
        return Result.Failure<EmbeddingDto>(ClientErrors.EmptyEmbeddings.Build());
    }

    private static Result TextCanBeProcessable(EmbeddingText text)
    {
        // Tokenizer is a simple library that can be used to count the number of tokens in a string.
        // https://github.com/Microsoft/Tokenizer -> Alternative to 'SharpToken'
        
        var encoding = GptEncoding.GetEncodingForModel(EmbeddingModel);
        var tokens = encoding.CountTokens(text.Text);

        if (tokens > MaxTokens)
        {
            Result.Failure<EmbeddingDto>(ClientErrors.TooManyTokens.Build());
        }

        return Result.Success();
    }
}