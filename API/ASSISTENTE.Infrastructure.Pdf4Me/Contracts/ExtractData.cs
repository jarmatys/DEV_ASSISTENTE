using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Pdf4Me.Contracts;

public class ExtractData
{
    private ExtractData(string text, List<PageContent> pages)
    {
        Text = text;
        Pages = pages;
    }

    public string Text { get; }
    public List<PageContent> Pages { get; }

    public static Result<ExtractData> Create(string text, List<PageContent> pages)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Result.Failure<ExtractData>("Text cannot be empty.");

        if (pages.Count == 0)
            return Result.Failure<ExtractData>("PDF file cannot be empty.");
        
        return Result.Success(new ExtractData(text, pages));
    }
}