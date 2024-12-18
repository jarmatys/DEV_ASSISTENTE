using ASSISTENTE.Infrastructure.Langfuse.Contracts;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Langfuse;

internal sealed class LangfuseService() : ILangfuseService
{
    public async Task<Result> CreateIngestionAsync()
    {
        return Result.Success();
    }
}