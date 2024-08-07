using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Infrastructure.Embeddings.Errors;

internal static class OpenAiClientErrors
{
    public static readonly Error InvalidResult =
        new("OpenAiClient.InvalidResult", "Invalid result from OpenAI service");
    
    public static readonly Error EmptyEmbeddings =
        new("OpenAiClient.EmptyEmbeddings", "Empty embeddings from OpenAI service");
    
    public static readonly Error TooManyTokens =
        new("OpenAiClient.TooManyTokens", "Too many tokens in the text");
}