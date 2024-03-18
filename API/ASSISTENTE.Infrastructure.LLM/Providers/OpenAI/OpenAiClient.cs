using ASSISTENTE.Infrastructure.LLM.Errors;
using ASSISTENTE.Infrastructure.LLM.Models;
using ASSISTENTE.Infrastructure.LLM.ValueObjects;
using CSharpFunctionalExtensions;
using OpenAI.Net;
using OpenAI.Net.Models.Requests;

namespace ASSISTENTE.Infrastructure.LLM.Providers.OpenAI;

internal sealed class OpenAiClient(IOpenAIService openAiService) : ILLMClient
{
    public async Task<Result<AnswerDto>> GenerateAnswer(PromptText prompt)
    {
        var message = Message.Create("user", prompt.Value);
        var completionRequest = new ChatCompletionRequest("gpt-4", message)
        {
            MaxTokens = 100
        };

        // TODO: Fill role with the correct value
        
        var response = await openAiService.Chat.Get(completionRequest);
        
        if (!response.IsSuccess)
            return Result.Failure<AnswerDto>(OpenAiClientErrors.InvalidResult.Build(response.ErrorResponse?.Error.Message!));

        var answer = response.Result?.Choices.FirstOrDefault()?.Message.Content;
        
        return answer is null
            ? Result.Failure<AnswerDto>(OpenAiClientErrors.EmptyAnswer.Build())
            : Result.Success(AnswerDto.Create(answer));
    }
}