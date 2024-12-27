using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.LLM.Contracts;

public interface ILlmClient
{
    Task<Result<Answer>> GenerateAnswer(Prompt prompt);
    Task<Result> FineTune(FineTuning fineTuning);
}