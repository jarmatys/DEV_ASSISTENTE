using ASSISTENTE.Infrastructure.Qdrant.Configurations;
using ASSISTENTE.Infrastructure.Qdrant.Contracts;
using ASSISTENTE.Infrastructure.Qdrant.Errors;
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

    public async Task<Result> DropCollectionAsync(string name)
    {
        var collections = await client.ListCollectionsAsync();

        if (!collections.Contains(name)) return Result.Success();

        await client.DeleteCollectionAsync(name);

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

    public async Task<Result<List<SearchResult>>> SearchAsync(VectorDto vectorDto)
    {
        try
        {
            var searchParams = new SearchParams
                { };

            var filter = new Filter
            {
                Must = { },
                Should = { }
            };

            var response = await client.SearchAsync(
                collectionName: vectorDto.GetCollectionName(),
                vector: vectorDto.GetVector(),
                limit: (ulong)vectorDto.Elements,
                searchParams: searchParams,
                filter: filter
            );

            var result = response.Select(x => SearchResult.Create(x.Id, x.Score, x.Payload)).ToList();

            return result.Count == 0
                ? Result.Failure<List<SearchResult>>(QdrantServiceErrors.MissingResources.Build())
                : result;
        }
        catch (Exception e)
        {
            return Result.Failure<List<SearchResult>>(QdrantServiceErrors.ConnectionFailed.Build(e.Message));
        }
    }
}