using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.LLM.Contracts;

public sealed class FineTuning : ValueObject
{
    private FineTuning(string filePath)
    {
        FilePath = filePath;
    }
    
    public string FilePath { get; }
    
    public static Result<FineTuning> Create(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return Result.Failure<FineTuning>(CommonErrors.EmptyParameter.Build());
        
        return new FineTuning(filePath);
    }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return FilePath;
    }
}