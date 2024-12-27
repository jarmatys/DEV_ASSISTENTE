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

    public async Task<Result<string>> InitCrawlAsync(string website)
    {
        var crawlJob = await client.Crawling.CrawlUrlsAsync(website);

        if (crawlJob.JobId == null)
            return Result.Failure<string>("Crawl job failed to start.");

        var crawlResult = await client.Crawl.WaitJobAsync(crawlJob.JobId);

        if (crawlResult.Data == null)
            return Result.Failure<string>("Crawl job failed to complete.");

        var urls = crawlResult.Data?.Select(x => x.Url);

        return Result.Success("");
    }

    public async Task<Result<List<PageDetails>>> CrawlResultAsync(string jobId)
    {
        var crawlJob = await client.Crawl.GetCrawlStatusAsync("cf7e3926-722f-48c8-99b0-e30763332f88");

        if (crawlJob.Data == null || crawlJob.Status != "completed")
            return Result.Failure<List<PageDetails>>("Crawl job failed to complete.");

        var pagesDetails = crawlJob.Data.Select(x =>
                PageDetails.Create(
                    url: x.Metadata?.SourceURL,
                    title: x.Metadata?.Title,
                    content: x.Markdown,
                    links: x.AdditionalProperties["linksOnPage"].ToString() ?? string.Empty
                )
            )
            .ToList();

        if (pagesDetails.Any(x => x.IsFailure))
            return Result.Failure<List<PageDetails>>("Failed to create page details.");

        var result = pagesDetails.Select(x => x.Value).ToList();

        return Result.Success(result);
    }
}