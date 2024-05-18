using ASSISTENTE.Infrastructure.PromptGenerator.Errors;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;
using ASSISTENTE.Infrastructure.PromptGenerator.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator;

internal class PromptFactory(IEnumerable<IPrompt> prompts) : IPromptFactory
{
    public Result<string> GeneratePrompt(PromptInput input)
    {
        var prompt = prompts.FirstOrDefault(p => p.Type == input.Type);
        
        return prompt == null 
            ? Result.Failure<string>(PromptFactoryErrors.PromptTypeNotSupported.Build()) 
            : Result.Success(prompt.Generate(input.Prompt, input.Context));
    }
}