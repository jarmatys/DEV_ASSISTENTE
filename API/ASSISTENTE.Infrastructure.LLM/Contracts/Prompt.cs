using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.LLM.Contracts;

public sealed class Prompt : ValueObject
{
    private Prompt(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<Prompt> Create(string prompt)
    {
        if (string.IsNullOrEmpty(prompt))
            return Result.Failure<Prompt>(CommonErrors.EmptyParameter.Build());
        
        return new Prompt(prompt);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}