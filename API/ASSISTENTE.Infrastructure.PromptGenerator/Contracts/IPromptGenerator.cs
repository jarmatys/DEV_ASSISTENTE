using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Contracts;

public interface IPromptGenerator
{
    Result<string> GeneratePrompt(PromptInput input);
}