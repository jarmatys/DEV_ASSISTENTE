using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Application.Abstractions.ValueObjects;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Enums;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Enums;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Embeddings.ValueObjects;
using ASSISTENTE.Infrastructure.Errors;
using ASSISTENTE.Infrastructure.LLM;
using ASSISTENTE.Infrastructure.LLM.ValueObjects;
using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;
using ASSISTENTE.Infrastructure.PromptGenerator.ValueObjects;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Qdrant.Models;

using AnswerEntity = ASSISTENTE.Domain.Entities.Answers.Answer;

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

    public async Task<Result<string>> RecallAsync(string questionText, string? connectionId)
    {
        // TODO: TODO: RecallAsync will be an aggregator of all methods and will be generate answer synchronous 
        // TODO: split this method into smaller methods
        var answer = await ResolveQuestionContext(questionText, connectionId) // Step 1 & 2
            .Bind(async question =>
            {
                var llmResult = await EmbeddingText.Create(questionText) // 3. Create embedding
                    .Bind(embeddingClient.GetAsync)
                    .Check(dto => question.AddEmbeddings(dto.Embeddings))
                    .Bind(dto => VectorDto.Create(CollectionName(question.GetContext()), dto.Embeddings))
                    .Bind(qdrantService.SearchAsync) // 4. Search for resources
                    .Bind(searchResult =>
                    {
                        var resourceIds = searchResult.Select(x => x.ResourceId);

                        return resourceRepository
                            .FindByResourceIdsAsync(resourceIds)
                            .ToResult(KnowledgeServiceErrors.NotFound.Build());
                    })
                    .Check(question.AddResource) // 5. Save selected resources to question (save in DB for audit purpose)
                    .Bind(resources =>
                    {
                        var contextContent = resources.Select(x => x.Content);

                        return GetPromptType(question.Context) // 6. Generate prompt
                            .Bind(promptType => PromptInput.Create(questionText, contextContent, promptType))
                            .Bind(promptGenerator.GeneratePrompt);
                    })
                    .Bind(Prompt.Create)
                    .Bind(prompt =>
                    {
                        var answer = llmClient.GenerateAnswer(prompt) // 7. Generate answer
                            .Check(answer => AnswerEntity.Create(
                                    answer.Text,
                                    prompt.Value,
                                    answer.Client.Name,
                                    answer.Audit.Model,
                                    answer.Audit.PromptTokens,
                                    answer.Audit.CompletionTokens
                                )
                                .Check(question.SetAnswer)); // 8. Save answer to question (save in DB for audit purpose)
                        
                        return answer;
                    })
                    .Check(_ => questionRepository.UpdateAsync(question)); // 9. Commit DB transation
                
                return llmResult;
            })
            .Map(answer => answer.Text);
        
        return answer;
    }

    public async Task<Result<Question>> ResolveQuestionContext(string questionText, string? connectionId)
    {
        return await GetContextAsync<ResourceType, QuestionContext>(questionText) // 1. Detect context
            .Bind(async context =>
            {
                var question = Question.Create(questionText, connectionId, context); // 2. Init question (save in DB for audit purpose)

                if (context == QuestionContext.Error)
                {
                    return await question
                        .Map(questionRepository.AddAsync)
                        .Finally(_ => Result.Failure<Question>(KnowledgeServiceErrors.ContextNotRecognized.Build()));
                }

                return question;
            })
            .Check(questionRepository.AddAsync);
    }
    
    private async Task<Result<TContext>> GetContextAsync<TType, TContext>(string question) 
        where TType : struct, Enum
        where TContext : struct, Enum
    {
        var promptText = sourceProvider.Prompt<TType>(question);

        var result = await Prompt.Create(promptText)
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
    
    private static Result<PromptType> GetPromptType(QuestionContext context)
    {
        return context switch
        {
            QuestionContext.Note => PromptType.Question,
            QuestionContext.Code => PromptType.Code,
            _ => Result.Failure<PromptType>(KnowledgeServiceErrors.PromptTypeNotExist.Build())
        };
    }
}