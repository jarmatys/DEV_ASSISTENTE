using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator.ValueObjects;

public sealed class PromptInput
{
    private PromptInput(string question, string context, PromptType type)
    {
        Question = question;
        Context = context;
        Type = type;
    }
    
    public string Question { get; }
    public string Context { get; }
    public PromptType Type { get; }
    
    public static Result<PromptInput> Create(string question, string context, PromptType type)
    {
        return new PromptInput(question, context, type);
    }
}