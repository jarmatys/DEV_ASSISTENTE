using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.MarkDownParser.Contracts;

public sealed class MediaUrl : ValueObject
{
    private static readonly List<string> _supportedExtensions =
    [
        "png",
        "jpg",
        "mp3"
    ];

    public string Url { get; }
    public string Extension { get; }

    private MediaUrl(string url, string extension)
    {
        Url = url;
        Extension = extension;
    }

    public static Result<MediaUrl> Create(string url)
    {
        if (string.IsNullOrEmpty(url))
            return Result.Failure<MediaUrl>(CommonErrors.EmptyParameter.Build());

        var extension = url.Split('.').Last();

        return _supportedExtensions.Contains(extension)
            ? new MediaUrl(url, extension)
            : Result.Failure<MediaUrl>(MediaUrlErrors.InvalidFileExtension.Build());
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Url;
    }
}

public static class MediaUrlErrors
{
    public static readonly Error InvalidFileExtension = new(
        "MediaUrl.InvalidFileExtension", "Invalid file extension - only '.png, .jpg, .mp3' file types are allowed");
}