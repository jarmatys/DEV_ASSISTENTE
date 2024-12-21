using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.Vision.Contracts;

public sealed class Image : ValueObject
{
    private Image(string prompt, string imageUrl)
    {
        Prompt = prompt;
        ImageUrl = imageUrl;
    }
    
    public string Prompt { get; }
    public string ImageUrl { get; }
    
    public static Result<Image> Create(string prompt, string imageBase64, string extension)
    {
        if (string.IsNullOrEmpty(imageBase64))
            return Result.Failure<Image>(CommonErrors.EmptyParameter.Build());
        
        if (string.IsNullOrEmpty(extension))
            return Result.Failure<Image>(CommonErrors.EmptyParameter.Build());

        var imageUrl = $"data:image/{extension.Replace(".", "")};base64,{imageBase64}";
        
        return new Image(prompt, imageUrl);
    }
    
    public static Result<Image> Create(string prompt, string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return Result.Failure<Image>(CommonErrors.EmptyParameter.Build());
        
        return new Image(prompt, imageUrl);
    }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return ImageUrl;
    }
}