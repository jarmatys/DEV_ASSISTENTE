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

    public async Task<Result<string>> RecallAsync(string questionText)
    {
        var answer = await GetContextAsync<ResourceType, QuestionContext>(questionText)
            .Bind(async context =>
            {
                var question = Question.Create(questionText, context);
                
                if (context == QuestionContext.Error)
                {
                    return await question
                        .Map(questionRepository.AddAsync)
                        .Finally(_ => Result.Failure<Question>(KnowledgeServiceErrors.ContextNotRecognized.Build()));
                }
                
                return question;
                
            })
            .Bind(async question =>
            {
                var llmResult = await EmbeddingText.Create(questionText)
                    .Bind(embeddingClient.GetAsync)
                    .Check(dto => question.AddEmbeddings(dto.Embeddings))
                    .Bind(dto => VectorDto.Create(CollectionName(question.GetContext()), dto.Embeddings))
                    .Bind(qdrantService.SearchAsync)
                    .Bind(searchResult =>
                    {
                        var resourceIds = searchResult.Select(x => x.ResourceId);

                        return resourceRepository
                            .FindByResourceIdsAsync(resourceIds)
                            .ToResult(KnowledgeServiceErrors.NotFound.Build());
                    })
                    .Check(question.AddResource)
                    .Bind(resources =>
                    {
                        var contextContent = resources.Select(x => x.Content);

                        return promptGenerator
                            .GeneratePrompt(questionText, contextContent, PromptType.Question);
                    })
                    .Check(_ => questionRepository.AddAsync(question))
                    .Bind(PromptText.Create)
                    .Bind(llmClient.GenerateAnswer);
                
                return llmResult;
            })
            .Map(answer => answer.Text);
        
        return answer;
    }

    private async Task<Result<TContext>> GetContextAsync<TType, TContext>(string question) 
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