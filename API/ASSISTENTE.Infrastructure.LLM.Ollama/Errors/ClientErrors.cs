using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.LLM.Ollama.Errors;

internal static class ClientErrors
{
    public static readonly Error EmptyAnswer =
        new("ClientErrors.EmptyAnswer", "Empty answer from Ollama service");
}