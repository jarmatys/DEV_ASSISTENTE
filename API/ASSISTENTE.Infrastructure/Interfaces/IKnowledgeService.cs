using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Interfaces;

public interface IKnowledgeService
{
    public Task<Result> LearnAsync(string information);
    public Task<Result<string>> RecallAsync(string information);
}