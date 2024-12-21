using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.Image.Contracts;

public sealed class ImagePrompt : ValueObject
{
    private ImagePrompt(string prompt)
    {
        Prompt = prompt;
    }
    
    public string Prompt { get; }
    
    public static Result<ImagePrompt> Create(string prompt)
    {
        if (string.IsNullOrEmpty(prompt))
            return Result.Failure<ImagePrompt>(CommonErrors.EmptyParameter.Build());
        
        return new ImagePrompt(prompt);
    }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Prompt;
    }
}