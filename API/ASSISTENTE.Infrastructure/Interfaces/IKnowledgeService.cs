using ASSISTENTE.Domain.Enums;
using ASSISTENTE.Infrastructure.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Interfaces;

public interface IKnowledgeService
{
    public Task<Result> LearnAsync(ResourceText text, ResourceType type);
    public Task<Result<string>> RecallAsync(string question);
}