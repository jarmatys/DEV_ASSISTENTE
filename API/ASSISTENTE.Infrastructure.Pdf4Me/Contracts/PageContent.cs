using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Pdf4Me.Contracts;

public class PageContent
{
    private PageContent(string text, List<byte[]> images)
    {
        Text = text;
        Images = images;
    }

    public string Text { get; }
    public List<byte[]> Images { get; }
    public List<string> ImagesBase64 => Images.Select(Convert.ToBase64String).ToList();
    
    public static Result<PageContent> Create(string text, List<byte[]>? images)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Result.Failure<PageContent>("Text cannot be empty.");
        
        return new PageContent(text, images ?? []);
    }
}