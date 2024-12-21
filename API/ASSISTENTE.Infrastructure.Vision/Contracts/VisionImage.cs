using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.Vision.Contracts;

public sealed class VisionImage : ValueObject
{
    private VisionImage(string prompt, string imageUrl)
    {
        Prompt = prompt;
        ImageUrl = imageUrl;
    }
    
    public string Prompt { get; }
    public string ImageUrl { get; }
    
    public static Result<VisionImage> Create(string prompt, string imageBase64, string extension)
    {
        if (string.IsNullOrEmpty(imageBase64))
            return Result.Failure<VisionImage>(CommonErrors.EmptyParameter.Build());
        
        if (string.IsNullOrEmpty(extension))
            return Result.Failure<VisionImage>(CommonErrors.EmptyParameter.Build());

        var imageUrl = $"data:image/{extension.Replace(".", "")};base64,{imageBase64}";
        
        return new VisionImage(prompt, imageUrl);
    }
    
    public static Result<VisionImage> Create(string prompt, string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return Result.Failure<VisionImage>(CommonErrors.EmptyParameter.Build());
        
        return new VisionImage(prompt, imageUrl);
    }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return ImageUrl;
    }
}