using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Domain.Enums;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Embeddings.ValueObjects;
using ASSISTENTE.Infrastructure.Interfaces;
using ASSISTENTE.Infrastructure.LLM;
using ASSISTENTE.Infrastructure.LLM.ValueObjects;
using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Qdrant.Models;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Services;

public sealed class KnowledgeService(
    IEmbeddingClient embeddingClient,
    IQdrantService qdrantService,
    IPromptGenerator promptGenerator,
    IResourceRepository resourceRepository,
    ILLMClient llmClient
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

    public async Task<Result<string>> RecallAsync(string question)
    {
        var searchEmbeddings = await EmbeddingText.Create(question)
            .Bind(embeddingClient.GetAsync)
            .Map(embedding => embedding)
            .TapError(errors => Console.WriteLine(errors));

        var searchResult = await VectorDto.Create("embeddings", searchEmbeddings.Value.Embeddings)
            .Bind(qdrantService.SearchAsync)
            .TapError(errors => Console.WriteLine(errors));

        var resource = await resourceRepository.FindByResourceIdAsync(searchResult.Value.ResourceId)
            .GetValueOrThrow();

        var prompt = promptGenerator.GeneratePrompt(
            question,
            context: resource.Content,
            PromptType.Question);

        var answer = await PromptText.Create(prompt.Value)
            .Bind(llmClient.GenerateAnswer);
        
        return answer.Value.Text;
    }
}