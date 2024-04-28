using ASSISTENTE.Application.Abstractions.ValueObjects;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Resources.Enums;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Application.Abstractions.Interfaces;

public interface IKnowledgeService
{
    public Task<Result> LearnAsync(ResourceText text, ResourceType type);
    public Task<Result<string>> AnswerAsync(string questionText);
    
    public Task<Result<Question>> InitQuestion(string questionText, string? connectionId);
    public Task<Result> ResolveContext(Question question);
    public Task<Result> FindResources(Question question);
    public Task<Result<string>> GenerateAnswer(Question question);
}