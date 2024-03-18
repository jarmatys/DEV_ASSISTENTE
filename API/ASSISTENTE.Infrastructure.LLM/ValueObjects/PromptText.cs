using ASSISTENTE.Common.Results;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.LLM.ValueObjects;

public sealed class PromptText
{
    public string Value { get; }

    private PromptText(string value)
    {
        Value = value;
    }
    
    public static Result<PromptText> Create(string prompt)
    {
        if (string.IsNullOrEmpty(prompt))
            return Result.Failure<PromptText>(EmbeddingTextErrors.EmptyContent.Build());
        
        return new PromptText(prompt);
    }
}

public static class EmbeddingTextErrors
{
    public static readonly Error EmptyContent = new(
        "PromptText.EmptyContent", "Prompt cannot be empty.");
}