using ASSISTENTE.Infrastructure.Firecrawl.Contracts;
using CSharpFunctionalExtensions;
using Firecrawl;

namespace ASSISTENTE.Infrastructure.Firecrawl;

internal sealed class FirecrawlService(FirecrawlApp client) : IFirecrawlService
{
    public async Task<Result<string>> ScrapeAsync(string website)
    {
        var response = await client.Scraping.ScrapeAsync(website);

        if (!response.Success)
            return Result.Failure<string>(response.Warning);

        var markdown = response.Data?.Markdown;
        
        if (string.IsNullOrEmpty(markdown))
            return Result.Failure<string>("No markdown content found.");

        return Result.Success(markdown);
    }
}