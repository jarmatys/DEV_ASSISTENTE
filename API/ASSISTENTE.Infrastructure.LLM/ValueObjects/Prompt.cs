using ASSISTENTE.Common.Results;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.LLM.ValueObjects;

public sealed class Prompt
{
    private Prompt(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<Prompt> Create(string prompt)
    {
        if (string.IsNullOrEmpty(prompt))
            return Result.Failure<Prompt>(EmbeddingTextErrors.EmptyContent.Build());
        
        return new Prompt(prompt);
    }
}

public static class EmbeddingTextErrors
{
    public static readonly Error EmptyContent = new(
        "Prompt.EmptyContent", "Prompt cannot be empty.");
}