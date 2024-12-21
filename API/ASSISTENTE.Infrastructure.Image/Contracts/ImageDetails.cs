using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.Image.Contracts;

public sealed class ImageDetails : ValueObject
{
    private ImageDetails(string imageUrl)
    {
        ImageUrl = imageUrl;
    }

    public string ImageUrl { get; }

    public static Result<ImageDetails> Create(string? imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return Result.Failure<ImageDetails>(CommonErrors.EmptyParameter.Build());

        return new ImageDetails(imageUrl);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return ImageUrl;
    }
}