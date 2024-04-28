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
using ASSISTENTE.Language.Identifiers;
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

    public async Task<Result<string>> AnswerAsync(string questionText)
    {
        // STEPS:
        // 1. Create question (save in DB for audit purpose) - connectionId + questionText
        // 2. Resolve question context 
        // 3. Find resources and save for audit purpose
        // 4. Generate prompt and answer + save for audit purpose

        return await InitQuestion(questionText, connectionId: null) // Step 1 
            .Bind(async question =>
            {
                return await ResolveContext(question) // Step 2
                    .Bind(async () => await FindResources(question)) // Step 3 & 4 & 5
                    .Bind(async () => await GenerateAnswer(question)); // Step 6 & 7 & 8 & 9
            });
    }

    public async Task<Result<Question>> InitQuestion(string questionText, string? connectionId)
    {
        return await Question.Create(questionText, connectionId) // 2. Init question (save in DB for audit purpose)
            .Check(questionRepository.AddAsync);
    }

    public async Task<Result> ResolveContext(Question question)
    {
        return await GetContextAsync<ResourceType, QuestionContext>(question.Text) // 1. Detect context
            .Check(question.SetContext)
            .Check(_ => questionRepository.UpdateAsync(question));
    }

    public async Task<Result> FindResources(Question question)
    {
        // TODO: verify context (shound't be Error) | domain validation in question.GetContext()

        return await EmbeddingText.Create(question.Text) // 3. Create embedding
            .Bind(embeddingClient.GetAsync)
            .Check(dto => question.AddEmbeddings(dto.Embeddings))
            .Bind(dto => VectorDto.Create(CollectionName(question.GetContext()), dto.Embeddings))
            .Bind(qdrantService.SearchAsync) // 4. Search for resources
            .Bind(searchResult =>
            {
                var resourceIds = searchResult.Select(x => new ResourceId(x.ResourceId));

                return resourceRepository
                    .FindByResourceIdsAsync(resourceIds)
                    .ToResult(KnowledgeServiceErrors.NotFound.Build());
            })
            .Check(question.AddResource) // 5. Save selected resources to question (save in DB for audit purpose)
            .Check(_ => questionRepository.UpdateAsync(question));
    }

    public async Task<Result<string>> GenerateAnswer(Question question)
    {
        var contextContent = question.Resources.Select(x => x.Resource).Select(x => x.Content);

        // TODO: verify context (shound't be Error) | domain validation in question.GetContext()
        
        return await GetPromptType(question.Context.Value) // 6. Generate prompt
            .Bind(promptType => PromptInput.Create(question.Text, contextContent, promptType))
            .Bind(promptGenerator.GeneratePrompt)
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
            .Check(_ => questionRepository.UpdateAsync(question)) // 9. Commit DB transation
            .Map(answer => answer.Text);
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