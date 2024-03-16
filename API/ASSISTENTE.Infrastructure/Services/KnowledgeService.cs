using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Domain.Enums;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Embeddings.ValueObjects;
using ASSISTENTE.Infrastructure.Interfaces;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Qdrant.Models;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Services;

public sealed class KnowledgeService(
    IEmbeddingClient embeddingClient,
    IQdrantService qdrantService,
    IResourceRepository resourceRepository
) : IKnowledgeService
{
    public async Task<Result> LearnAsync(string information, ResourceType type)
    {
        var resource = await Resource.Create(information, type)
            .Bind(resourceRepository.AddAsync)
            .TapError(errors => Console.WriteLine(errors));
        
        var embeddings = await EmbeddingText.Create(information)
            .Bind(embeddingClient.GetAsync)
            .Map(embedding => embedding)
            .TapError(errors => Console.WriteLine(errors));

        var upsertResult = await DocumentDto.Create(
                "embeddings",
                embeddings.Value.Embeddings,
                resource.Value.ResourceId
            )
            .Bind(qdrantService.UpsertAsync)
            .TapError(errors => Console.WriteLine(errors));

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