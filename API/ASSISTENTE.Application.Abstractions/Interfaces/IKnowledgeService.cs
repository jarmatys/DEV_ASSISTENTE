using ASSISTENTE.Application.Abstractions.ValueObjects;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Resources.Enums;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Application.Abstractions.Interfaces;

public interface IKnowledgeService
{
    public Task<Result> LearnAsync(ResourceText text, ResourceType type);
    public Task<Result<string>> RecallAsync(string questionText, string? connectionId);
    public Task<Result<Question>> ResolveQuestionContext(string questionText, string? connectionId);
}