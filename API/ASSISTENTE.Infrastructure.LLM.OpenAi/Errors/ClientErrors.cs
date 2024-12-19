using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.LLM.OpenAi.Errors;

internal static class ClientErrors
{
    public static readonly Error EmptyEmbeddings =
        new("ClientErrors.EmptyEmbeddings", "Empty embeddings from OpenAI service");
    
    public static readonly Error TooManyTokens =
        new("ClientErrors.TooManyTokens", "Too many tokens in the text");
    
    public static readonly Error InvalidResult =
        new("ClientErrors.InvalidResult", "Invalid result from OpenAI service");
    
    public static readonly Error EmptyAnswer =
        new("ClientErrors.EmptyAnswer", "Empty answer from OpenAI service");
}