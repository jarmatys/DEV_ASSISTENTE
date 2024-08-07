using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.LLM.Contracts;

public sealed class Audit : ValueObject
{
    private Audit(string model, int completionTokens, int promptTokens)
    {
        Model = model;
        CompletionTokens = completionTokens;
        PromptTokens = promptTokens;
    }

    public string Model { get; }
    public int CompletionTokens { get; }
    public int PromptTokens { get; }
    
    public static Result<Audit> Create(
        string? model, 
        int? completionTokens, 
        int? promptTokens)
    {
        if (string.IsNullOrEmpty(model))
            return Result.Failure<Audit>(CommonErrors.EmptyParameter.Build());
            
        if (!completionTokens.HasValue)
            return Result.Failure<Audit>(CommonErrors.EmptyParameter.Build());

        if (!promptTokens.HasValue)
            return Result.Failure<Audit>(CommonErrors.EmptyParameter.Build());
        
        return new Audit(model, completionTokens.Value, promptTokens.Value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Model;
        yield return CompletionTokens;
        yield return PromptTokens;
    }
}