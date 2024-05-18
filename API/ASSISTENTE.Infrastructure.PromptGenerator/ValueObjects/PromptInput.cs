using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.PromptGenerator.ValueObjects;

public sealed class PromptInput
{
    private PromptInput(string prompt, IEnumerable<string> context, PromptType type)
    {
        Prompt = prompt;
        Context = context;
        Type = type;
    }
    
    public string Prompt { get; }
    public IEnumerable<string> Context { get; }
    public PromptType Type { get; }
    
    public static Result<PromptInput> Create(string prompt, IEnumerable<string> context, PromptType type)
    {
        return new PromptInput(prompt, context, type);
    }
}