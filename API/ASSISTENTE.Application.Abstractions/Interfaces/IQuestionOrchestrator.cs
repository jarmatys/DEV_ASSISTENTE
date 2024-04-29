using ASSISTENTE.Domain.Entities.Questions;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Application.Abstractions.Interfaces;

public interface IQuestionOrchestrator
{
    public Task<Result<Question>> InitQuestion(string questionText, string? connectionId);
    public Task<Result> ResolveContext(Question question);
    public Task<Result> FindResources(Question question);
    public Task<Result<string>> GenerateAnswer(Question question);
}