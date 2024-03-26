using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Domain.Enums;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Embeddings.ValueObjects;
using ASSISTENTE.Infrastructure.Errors;
using ASSISTENTE.Infrastructure.Interfaces;
using ASSISTENTE.Infrastructure.LLM;
using ASSISTENTE.Infrastructure.LLM.ValueObjects;
using ASSISTENTE.Infrastructure.PromptGenerator;
using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Qdrant.Models;
using ASSISTENTE.Infrastructure.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Services;

public sealed class KnowledgeService(
    IEmbeddingClient embeddingClient,
    IQdrantService qdrantService,
    IPromptGenerator promptGenerator,
    IResourceRepository resourceRepository,
    ILLMClient llmClient,
    ISourceProvider sourceProvider
) : IKnowledgeService
{
    private const string CollectionName = "embeddings";

    public async Task<Result> LearnAsync(ResourceText text, ResourceType type)
    {
        // TODO: Create embeddings collection for each resource type

        var resource = await Resource.Create(text.Content, text.Title, type)
            .Bind(resourceRepository.AddAsync)
            .TapError(errors => Console.WriteLine(errors));

        var embeddings = await EmbeddingText.Create(text.Content)
            .Bind(embeddingClient.GetAsync)
            .Map(embedding => embedding)
            .TapError(errors => Console.WriteLine(errors));

        return await DocumentDto.Create(
                CollectionName,
                embeddings.Value.Embeddings,
                resource.Value.ResourceId
            )
            .Bind(qdrantService.UpsertAsync)
            .TapError(errors => Console.WriteLine(errors));
    }

    public async Task<Result<string>> RecallAsync(string question)
    {
        var resourceType = await GetQuestionTypeAsync<ResourceType>(question);

        var answer = await EmbeddingText.Create(question)
            .Bind(embeddingClient.GetAsync)
            .Bind(dto => VectorDto.Create(CollectionName, dto.Embeddings))
            .Bind(qdrantService.SearchAsync)
            .Map(searchResult => searchResult.Select(x => x.ResourceId).ToList())
            .Map(resourceRepository.FindByResourceIdsAsync)
            .Ensure(resources => resources.HasValue, KnowledgeServiceErrors.NotFound.Build())
            .Map(resources => resources.Value.Select(x => x.Content))
            .Bind(context => promptGenerator.GeneratePrompt(question, context, PromptType.Question))
            .Bind(PromptText.Create)
            .Bind(llmClient.GenerateAnswer)
            .TapError(errors => Console.WriteLine(errors));

        return answer.Value.Text;
    }

    private async Task<Result<T>> GetQuestionTypeAsync<T>(string question) where T : struct, Enum
    {
        var promptText = sourceProvider.Prompt<T>(question);

        var result = await PromptText.Create(promptText)
            .Bind(llmClient.GenerateAnswer);

        var source = (T)Enum.Parse(typeof(T), result.Value.Text);
        
        return Result.Success(source);
    }
}