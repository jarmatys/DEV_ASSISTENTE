using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Application.Abstractions.ValueObjects;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Embeddings.Contracts;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Qdrant.Contracts;
using ASSISTENTE.Language.Enums;

namespace ASSISTENTE.Infrastructure.Services;

public sealed class KnowledgeService(
    IEmbeddingClient embeddingClient,
    IQdrantService qdrantService,
    IResourceRepository resourceRepository,
    IQuestionOrchestrator questionOrchestrator
) : IKnowledgeService
{
    private static string CollectionName(string type) => $"embeddings-{type}";

    public async Task<Result> LearnAsync(ResourceText text, ResourceType type)
    {
        var embeddingResult = await EmbeddingText.Create(text.Content)
            .Bind(embeddingClient.GetAsync);

        if (embeddingResult.IsFailure)
            return Result.Failure(embeddingResult.Error);

        var embeddings = embeddingResult.Value.Embeddings.ToList();

        var resourceResult = await Resource.Create(text.Content, text.Title, type, embeddings)
            .Bind(resourceRepository.AddAsync);

        if (resourceResult.IsFailure)
            return Result.Failure(resourceResult.Error);

        return await DocumentDto.Create(CollectionName(type.ToString()), embeddings, resourceResult.Value.Id)
            .Bind(qdrantService.UpsertAsync);
    }

    public async Task<Result<string>> AnswerAsync(string questionText)
    {
        return await questionOrchestrator.InitQuestion(questionText, connectionId: null)
            .Bind(async question =>
            {
                return await questionOrchestrator.ResolveContext(question)
                    .Bind(async () => await questionOrchestrator.FindResources(question))
                    .Bind(async () => await questionOrchestrator.GenerateAnswer(question));
            });
    }
}