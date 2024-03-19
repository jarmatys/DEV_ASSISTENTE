using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator;

internal sealed class PromptGenerator(IPromptFactory promptFactory) : IPromptGenerator
{
    public Result<string> GeneratePrompt(string question, IEnumerable<string> context, PromptType type)
    {
        return promptFactory.GeneratePrompt(question, context, type);
    }
}