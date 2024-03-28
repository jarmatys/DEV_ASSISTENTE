using ASSISTENTE.Domain.Entities.Resources.Enums;
using ASSISTENTE.Infrastructure.Interfaces;
using ASSISTENTE.Infrastructure.Qdrant;

namespace ASSISTENTE.Infrastructure.Services;

public sealed class MaintenanceService(
    IQdrantService qdrantService
) : IMaintenanceService
{
    public async Task<Result> InitAsync()
    {
        var results = new List<Result>();
        
        foreach( var resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            var result = await qdrantService.CreateCollectionAsync($"embeddings-{resourceType}");
            
            results.Add(result);
        }
        
        return Result.Combine(results);
    }

    public async Task<Result> ResetAsync()
    {
        var results = new List<Result>();
        
        foreach( var resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            var result = await qdrantService.DropCollectionAsync($"embeddings-{resourceType}");
            
            results.Add(result);
        }
        
        return Result.Combine(results);
    }
}