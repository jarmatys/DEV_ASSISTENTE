using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using ASSISTENTE.Infrastructure.PromptGenerator.Errors;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator;

internal class PromptFactory(IEnumerable<IPrompt> prompts) : IPromptFactory
{
    public Result<string> GeneratePrompt(string question, string context, PromptType type)
    {
        var prompt = prompts.FirstOrDefault(p => p.Type == type);
        
        return prompt == null 
            ? Result.Failure<string>(PromptFactoryErrors.PromptTypeNotSupported.Build()) 
            : Result.Success(prompt.Generate(question, context));
    }
}