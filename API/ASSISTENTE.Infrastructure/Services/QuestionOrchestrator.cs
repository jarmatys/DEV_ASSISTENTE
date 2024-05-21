using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
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
using ASSISTENTE.Language.Enums;
using ASSISTENTE.Language.Identifiers;
using AnswerEntity = ASSISTENTE.Domain.Entities.Answers.Answer;

namespace ASSISTENTE.Infrastructure.Services;

public sealed class QuestionOrchestrator(
    IEmbeddingClient embeddingClient,
    IQdrantService qdrantService,
    IPromptGenerator promptGenerator,
    IResourceRepository resourceRepository,
    IQuestionRepository questionRepository,
    ILLMClient llmClient,
    IContextProvider contextProvider
) : IQuestionOrchestrator
{
    private static string CollectionName(string type) => $"embeddings-{type}";

    public async Task<Result<Question>> InitQuestion(string questionText, string? connectionId)
    {
        return await Question.Create(questionText, connectionId) // 1. Init question (save in DB for audit purpose)
            .Check(questionRepository.AddAsync);
    }

    public async Task<Result> ResolveContext(Question question)
    {
        return await DetermineContextAsync<ResourceType, QuestionContext>(question.Text) // 2. Detect context
            .Check(question.AddContext)
            .Check(_ => questionRepository.UpdateAsync(question));
    }
    
    public async Task<Result> FindFiles(Question question)
    {
        return await question.GetContext()
            .Bind(GetResourceType)
            .Map(async resourceType => await resourceRepository
                .GetFileNames(resourceType)
                .ToResult(KnowledgeServiceErrors.NotFound.Build())
            )
            .Map(fileNames => string.Join(Environment.NewLine, fileNames))
            .Bind(promptContext => PromptInput.Create(question.Text, promptContext, PromptType.Files))
            .Bind(promptGenerator.GeneratePrompt)
            .Bind(Prompt.Create)
            .Bind(async prompt =>
            {
                var answer = await llmClient.GenerateAnswer(prompt)
                    .Check(answer => QuestionFile.Create(answer.Text)
                        .Check(question.AddFiles));

                return answer;
            })
            .Check(_ => questionRepository.UpdateAsync(question))
            .Map(answer => answer.Text);
    }
    
    public async Task<Result> CreateEmbedding(Question question)
    {
        return await question.BuildEmbeddableText()
            .Bind(EmbeddingText.Create) // 3. Create embedding
            .Bind(embeddingClient.GetAsync)
            .Check(dto => question.AddEmbeddings(dto.Embeddings))
            .Check(_ => questionRepository.UpdateAsync(question));
    }
    
    public async Task<Result> FindResources(Question question)
    {
        return await question.GetContext()
            .Bind(context =>
            {
                return question.GetEmbeddings()
                    .Bind(embeddings => VectorDto.Create(CollectionName(context), embeddings))
                    .Bind(qdrantService.SearchAsync) // 4. Search for resources
                    .Bind(searchResult =>
                    {
                        var resourceIds = searchResult.Select(x => new ResourceId(x.ResourceId));

                        return resourceRepository
                            .FindByResourceIdsAsync(resourceIds)
                            .ToResult(KnowledgeServiceErrors.NotFound.Build());
                    })
                    .Check(question.AddResources) // 5. Save selected resources to question
                    .Check(_ => questionRepository.UpdateAsync(question));
            });
    }

    public async Task<Result<string>> GenerateAnswer(Question question)
    {
        return await question.GetContext()
            .Bind(GetPromptType)
            .Bind(promptType =>
            {
                return question.BuildContext()
                    .Bind(promptContext => PromptInput.Create(question.Text, promptContext, promptType)) // 6. Generate prompt
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
                                .Check(question.AddAnswer)); // 8. Save answer to question 

                        return answer;
                    })
                    .Check(_ => questionRepository.UpdateAsync(question))
                    .Map(answer => answer.Text);
            });
    }

    private async Task<Result<TContext>> DetermineContextAsync<TType, TContext>(string question)
        where TType : struct, Enum
        where TContext : struct, Enum
    {
        var promptText = contextProvider.Prompt<TType>(question);

        var result = await Prompt.Create(promptText)
            .Bind(llmClient.GenerateAnswer);

        if (result.IsFailure)
            return Result.Failure<TContext>(result.Error);

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

    private static Result<PromptType> GetPromptType(string contextText)
    {
        var context = Enum.Parse<QuestionContext>(contextText);

        return context switch
        {
            QuestionContext.Note => PromptType.Question,
            QuestionContext.Code => PromptType.Code,
            _ => Result.Failure<PromptType>(KnowledgeServiceErrors.PromptTypeNotExist.Build())
        };
    }

    private static Result<ResourceType> GetResourceType(string contextText)
    {
        var context = Enum.Parse<QuestionContext>(contextText);

        return context switch
        {
            QuestionContext.Note => ResourceType.Note,
            QuestionContext.Code => ResourceType.Code,
            _ => Result.Failure<ResourceType>(KnowledgeServiceErrors.ResourceTypeNotExist.Build())
        };
    }
}