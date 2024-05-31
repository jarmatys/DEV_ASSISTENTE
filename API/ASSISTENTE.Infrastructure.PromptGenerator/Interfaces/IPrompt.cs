using ASSISTENTE.Infrastructure.PromptGenerator.Contracts;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;

internal interface IPrompt
{
    public PromptType Type { get; }
    public  string Generate(string question, string context);
}