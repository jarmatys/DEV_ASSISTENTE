using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Embeddings.ValueObjects;
using ASSISTENTE.Infrastructure.Interfaces;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Qdrant.Models;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Services;

public sealed class KnowledgeService(
    IEmbeddingClient embeddingClient,
    IQdrantService qdrantService
) : IKnowledgeService
{
    public async Task<Result> LearnAsync(string information)
    {
        var embeddings = await EmbeddingText.Create(information)
            .Bind(embeddingClient.GetAsync)
            .Map(embedding => embedding)
            .TapError(errors => Console.WriteLine(errors));
        
        await qdrantService.CreateCollectionAsync("embeddings"); // TODO: move this method to init method
        
        var upsertResult = await DocumentDto.Create("embeddings", embeddings.Value.Embeddings)
            .Bind(qdrantService.UpsertAsync)
            .TapError(errors => Console.WriteLine(errors));

        // TODO: save information in database
        
        return upsertResult;
    }

    public async Task<Result<string>> RecallAsync(string information)
    {
        var searchEmbeddings = await EmbeddingText.Create(information)
            .Bind(embeddingClient.GetAsync)
            .Map(embedding => embedding)
            .TapError(errors => Console.WriteLine(errors));
        
        var searchResult = await VectorDto.Create("embeddings", searchEmbeddings.Value.Embeddings)
            .Bind(qdrantService.SearchAsync)
            .TapError(errors => Console.WriteLine(errors));
        
        // TODO: get information from database
        
        return string.Empty; // TODO: will be implemented
    }
}