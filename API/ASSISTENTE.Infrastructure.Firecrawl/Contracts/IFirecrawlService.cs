using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Firecrawl.Contracts;

public interface IFirecrawlService
{
    Task<Result<string>> ScrapeAsync(string website);
    Task<Result<string>> InitCrawlAsync(string website);
    Task<Result<List<PageDetails>>> CrawlResultAsync(string jobId);
}