using ASSISTENTE.Infrastructure.LLM.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.LLM;

public interface ILLMClient
{
    Task<Result<Answer>> GenerateAnswer(Prompt prompt);
}