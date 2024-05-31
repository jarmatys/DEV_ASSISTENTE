using ASSISTENTE.Infrastructure.PromptGenerator.Contracts;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;

internal interface IPromptFactory
{
    Result<string> GeneratePrompt(PromptInput input);
}