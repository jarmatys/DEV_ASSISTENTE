using ASSISTENTE.Infrastructure.PromptGenerator.Contracts;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator;

internal sealed class PromptGenerator(IPromptFactory promptFactory) : IPromptGenerator
{
    public Result<string> GeneratePrompt(PromptInput input)
    {
        return promptFactory.GeneratePrompt(input);
    }
}