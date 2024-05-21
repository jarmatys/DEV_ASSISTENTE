using ASSISTENTE.Infrastructure.PromptGenerator.Enums;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;

public interface IPrompt
{
    public PromptType Type { get; }
    public  string Generate(string question, string context);
}