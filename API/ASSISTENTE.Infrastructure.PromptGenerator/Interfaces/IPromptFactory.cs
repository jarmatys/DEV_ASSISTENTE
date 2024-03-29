using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;

public interface IPromptFactory
{
    Result<string> GeneratePrompt(string question, IEnumerable<string> context, PromptType type);
}