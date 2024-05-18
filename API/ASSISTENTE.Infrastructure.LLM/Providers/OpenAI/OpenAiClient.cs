using ASSISTENTE.Infrastructure.LLM.Errors;
using ASSISTENTE.Infrastructure.LLM.Providers.OpenAI.Enums;
using ASSISTENTE.Infrastructure.LLM.ValueObjects;
using CSharpFunctionalExtensions;
using OpenAI.Net;
using OpenAI.Net.Models.Requests;

namespace ASSISTENTE.Infrastructure.LLM.Providers.OpenAI;

internal sealed class OpenAiClient(IOpenAIService openAiService) : ILLMClient
{
    private readonly LLMClient _llmClient = LLMClient.Create("openAi");
    
    // TODO: Add new model: https://openai.com/index/hello-gpt-4o/ (gpt-4o)
    private const string Gpt4 = "gpt-4";
    private const string Gpt3 = "gpt-3.5-turbo";

    public async Task<Result<Answer>> GenerateAnswer(Prompt prompt)
    {
        var message = CreateMessage(Role.User, prompt);

        var completionRequest = new ChatCompletionRequest(Gpt3, message)
        {
            MaxTokens = 4096,
        };

        var response = await openAiService.Chat.Get(completionRequest);

        if (!response.IsSuccess)
            return Result.Failure<Answer>(OpenAiClientErrors.InvalidResult.Build(response.ErrorResponse?.Error.Message!));

        var result = response.Result;
        if (result is null)
            return Result.Failure<Answer>(OpenAiClientErrors.EmptyAnswer.Build());

        var answer = response.Result?.Choices.FirstOrDefault()?.Message.Content;
        var model = response.Result?.Model;
        var promptTokens = response.Result?.Usage.Prompt_tokens;
        var completionTokens = response.Result?.Usage.Completion_tokens;
        
        return Audit.Create(model, promptTokens, completionTokens)
            .Bind(audit => Answer.Create(answer, prompt.Value, _llmClient, audit));
    }

    private static Message CreateMessage(Role role, Prompt prompt) =>
        Message.Create(role.ToString().ToLower(), prompt.Value);
}