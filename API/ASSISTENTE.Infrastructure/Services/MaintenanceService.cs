using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Language.Enums;
using ASSISTENTE.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Infrastructure.Services;

public sealed class MaintenanceService(
    IQdrantService qdrantService,
    IAssistenteDbContext context
) : IMaintenanceService
{
    public async Task<Result> InitAsync()
    {
        var resourceTypes = Enum.GetValues(typeof(ResourceType));
        var results = new List<Result>();

        var clearanceResult = await ClearDatabase();

        if (clearanceResult.IsFailure) return clearanceResult;
        
        foreach (var type in resourceTypes)
        {
            var result = await qdrantService.DropCollectionAsync($"embeddings-{type}");

            results.Add(result);
        }

        foreach (var type in resourceTypes)
        {
            var result = await qdrantService.CreateCollectionAsync($"embeddings-{type}");

            results.Add(result);
        }

        return Result.Combine(results);
    }

    private async Task<Result> ClearDatabase()
    {
        try
        {
            var resources = await context.Resources.ToListAsync();

            context.Resources.RemoveRange(resources);

            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch
        {
            return Result.Failure("Failed to clear database");
        }
    }
}