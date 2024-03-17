using ASSISTENTE.Domain.Enums;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Interfaces;

public interface IKnowledgeService
{
    public Task<Result> LearnAsync(string information, ResourceType type);
    public Task<Result<string>> RecallAsync(string question);
}