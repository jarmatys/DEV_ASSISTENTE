using ASSISTENTE.Domain.Entities.Enums;
using ASSISTENTE.Domain.Entities.Interfaces;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Enums;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Embeddings.ValueObjects;
using ASSISTENTE.Infrastructure.Errors;
using ASSISTENTE.Infrastructure.Interfaces;
using ASSISTENTE.Infrastructure.LLM;
using ASSISTENTE.Infrastructure.LLM.ValueObjects;
using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Qdrant.Models;
using ASSISTENTE.Infrastructure.ValueObjects;

namespace ASSISTENTE.Infrastructure.Services;

public sealed class KnowledgeService(
    IEmbeddingClient embeddingClient,
    IQdrantService qdrantService,
    IPromptGenerator promptGenerator,
    IResourceRepository resourceRepository,
    IQuestionRepository questionRepository,
    ILLMClient llmClient,
    ISourceProvider sourceProvider
) : IKnowledgeService
{
    private static string CollectionName(string type) => $"embeddings-{type}";

    public async Task<Result> LearnAsync(ResourceText text, ResourceType type)
    {
        var embeddingResult = await EmbeddingText.Create(text.Content)
            .Bind(embeddingClient.GetAsync)
            .Map(embedding => embedding)
            .TapError(errors => Console.WriteLine(errors));
        
        if (embeddingResult.IsFailure)
            return Result.Failure(embeddingResult.Error);

        var embeddings = embeddingResult.Value.Embeddings.ToList();
        
        var resource = await Resource.Create(text.Content, text.Title, type, embeddings)
            .Bind(resourceRepository.AddAsync)
            .TapError(errors => Console.WriteLine(errors));

        if (resource.IsFailure)
            return Result.Failure(resource.Error);
        
        return await DocumentDto.Create(CollectionName(type.ToString()), embeddings, resource.Value.ResourceId)
            .Bind(qdrantService.UpsertAsync)
            .TapError(errors => Console.WriteLine(errors));
    }

    public async Task<Result<string>> RecallAsync(string question)
    {
        var answer = await GetQuestionTypeAsync<ResourceType, QuestionContext>(question)
            .Map(async resourceType =>
            {
                // TODO: Implement question creation
                // var question = Question.Create(question, );
                //
                // if (resourceType == QuestionContext.Error)
                // {
                //     await questionRepository.AddAsync(question);
                //     
                //     return "I'm sorry, I don't understand the question.";
                // }
                
                var llmResult = await EmbeddingText.Create(question)
                    .Bind(embeddingClient.GetAsync)
                    .Bind(dto => VectorDto.Create(CollectionName(resourceType.ToString()), dto.Embeddings))
                    .Bind(qdrantService.SearchAsync)
                    .Map(searchResult => searchResult.Select(x => x.ResourceId).ToList())
                    .Map(resourceRepository.FindByResourceIdsAsync)
                    .Ensure(resources => resources.HasValue, KnowledgeServiceErrors.NotFound.Build())
                    .Map(resources => resources.Value.Select(x => x.Content))
                    .Bind(context => promptGenerator.GeneratePrompt(question, context, PromptType.Question))
                    .Bind(PromptText.Create)
                    .Bind(llmClient.GenerateAnswer)
                    .TapError(errors => Console.WriteLine(errors));

                return llmResult.Value.Text;
            });

        return answer;
    }

    private async Task<Result<TContext>> GetQuestionTypeAsync<TType, TContext>(string question) 
        where TType : struct, Enum
        where TContext : struct, Enum
    {
        var promptText = sourceProvider.Prompt<TType>(question);

        var result = await PromptText.Create(promptText)
            .Bind(llmClient.GenerateAnswer);

        try
        {
            var source = (TContext)Enum.Parse(typeof(TContext), result.Value.Text);
            return Result.Success(source);
        }
        catch (Exception)
        {
            return Result.Success(default(TContext));
        }
    }
}