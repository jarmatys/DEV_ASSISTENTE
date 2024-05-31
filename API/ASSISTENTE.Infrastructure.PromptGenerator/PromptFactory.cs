using ASSISTENTE.Infrastructure.PromptGenerator.Contracts;
using ASSISTENTE.Infrastructure.PromptGenerator.Errors;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator;

internal sealed class PromptFactory(IEnumerable<IPrompt> prompts) : IPromptFactory
{
    public Result<string> GeneratePrompt(PromptInput input)
    {
        var prompt = prompts.FirstOrDefault(p => p.Type == input.Type);
        
        return prompt == null 
            ? Result.Failure<string>(PromptFactoryErrors.PromptTypeNotSupported.Build()) 
            : Result.Success(prompt.Generate(input.Question, input.Context));
    }
}