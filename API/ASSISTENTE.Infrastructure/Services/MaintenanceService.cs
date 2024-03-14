using ASSISTENTE.Infrastructure.Interfaces;
using ASSISTENTE.Infrastructure.Qdrant;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Services;

public sealed class MaintenanceService(
    IQdrantService qdrantService
) : IMaintenanceService
{
    public async Task<Result> InitAsync()
    {
        var result = await qdrantService.CreateCollectionAsync("embeddings");

        return result;
    }

    public async Task<Result> ResetAsync()
    {
        var result = await qdrantService.DropCollectionAsync("embeddings");

        return result;
    }
}