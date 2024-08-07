using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.LLM.Errors;

internal static class OpenAiClientErrors
{
    public static readonly Error InvalidResult =
        new("OpenAiClient.InvalidResult", "Invalid result from OpenAI service");
    
    public static readonly Error EmptyAnswer =
        new("OpenAiClient.EmptyAnswer", "Empty answer from OpenAI service");
}