using ASSISTENTE.Application.Abstractions.ValueObjects;
using ASSISTENTE.Language.Enums;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Application.Abstractions.Interfaces;

public interface IKnowledgeService
{
    public Task<Result> LearnAsync(ResourceText text, ResourceType type);
    public Task<Result<string>> AnswerAsync(string questionText);
}