using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Langfuse.Contracts;

public interface ILangfuseService
{
    Task<Result> CreateIngestionAsync();
}