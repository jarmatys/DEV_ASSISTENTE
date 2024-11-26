using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Firecrawl.Contracts;

public interface IFirecrawlService
{
    Task<Result<string>> ScrapeAsync(string website);
}