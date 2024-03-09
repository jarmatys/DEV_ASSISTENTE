using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Infrastructure.Embeddings.Errors;

internal static class OpenAiClientErrors
{
    public static readonly Error InvalidResult =
        new("OpenAiClient.InvalidResult", "Invalid result from OpenAI service.");
}