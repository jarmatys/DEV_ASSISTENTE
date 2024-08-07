using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Errors;

internal static class PromptFactoryErrors
{
    public static readonly Error PromptTypeNotSupported =
        new("PromptFactory.PromptTypeNotSupported", "Prompt type not supported");
}