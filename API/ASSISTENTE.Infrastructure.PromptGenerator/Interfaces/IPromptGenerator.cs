using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;

public interface IPromptGenerator
{
    Result<string> GeneratePrompt(string question, IEnumerable<string> context, PromptType type);
}