using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Firecrawl.Contracts;

public class PageDetails
{
    private PageDetails(string url, string title, string content, string links)
    {
        Url = url;
        Title = title;
        Content = content;
        Links = links;
    }
    
    public string Url { get; }
    public string Title { get; }
    public string Content { get; }
    public string Links { get; }
    
    public static Result<PageDetails> Create(string? url, string? title, string? content, string? links)
    {
        if (string.IsNullOrEmpty(url))
            return Result.Failure<PageDetails>("Url is required.");
        
        if (string.IsNullOrEmpty(title))
            return Result.Failure<PageDetails>("Title is required.");
        
        if (string.IsNullOrEmpty(content))
            return Result.Failure<PageDetails>("Content is required.");
        
        if (string.IsNullOrEmpty(links))
            return Result.Failure<PageDetails>("Links are required.");
        
        return new PageDetails(url, title, content, links);
    }
}