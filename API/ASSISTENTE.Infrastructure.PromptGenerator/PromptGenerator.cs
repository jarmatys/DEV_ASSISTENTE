using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;
using ASSISTENTE.Infrastructure.PromptGenerator.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator;

internal sealed class PromptGenerator(IPromptFactory promptFactory) : IPromptGenerator
{
    public Result<string> GeneratePrompt(PromptInput input)
    {
        return promptFactory.GeneratePrompt(input);
    }
}