using ASSISTENTE.Infrastructure.PromptGenerator.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;

public interface IPromptFactory
{
    Result<string> GeneratePrompt(PromptInput input);
}