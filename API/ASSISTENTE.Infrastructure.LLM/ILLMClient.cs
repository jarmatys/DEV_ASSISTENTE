using ASSISTENTE.Infrastructure.LLM.Models;
using ASSISTENTE.Infrastructure.LLM.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.LLM;

public interface ILLMClient
{
    Task<Result<AnswerDto>> GenerateAnswer(PromptText prompt);

}