using ASSISTENTE.Infrastructure.Qdrant.Errors;
using ASSISTENTE.Infrastructure.Qdrant.Models;
using ASSISTENTE.Infrastructure.Qdrant.Settings;
using CSharpFunctionalExtensions;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace ASSISTENTE.Infrastructure.Qdrant;

internal sealed class QdrantService(QdrantClient client) : IQdrantService
{
    public async Task<Result> CreateCollectionAsync(string name)
    {
        var collections = await client.ListCollectionsAsync();

        if (collections.Contains(name)) return Result.Success();

        await client.CreateCollectionAsync(name, VectorConfiguration.Configuration);

        return Result.Success();
    }

    public async Task<Result> UpsertAsync(DocumentDto document)
    {
        var response = await client.UpsertAsync(
            document.GetCollectionName(),
            document.GetPoints()
        );

        return response.Status == UpdateStatus.Completed 
            ? Result.Success() 
            : Result.Failure(QdrantServiceErrors.UpsertFailed.Build());
    }

    public async Task<Result<SearchResult>> SearchAsync(VectorDto vectorDto)
    {
        throw new NotImplementedException();
    }
}