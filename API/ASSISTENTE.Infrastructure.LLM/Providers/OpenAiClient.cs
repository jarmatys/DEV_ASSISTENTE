using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.LLM.Errors;
using CSharpFunctionalExtensions;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using ChatMessage = OpenAI.Chat.Message;

namespace ASSISTENTE.Infrastructure.LLM.Providers;

internal sealed class OpenAiClient(OpenAIClient client) : ILlmClient
{
    private readonly LlmClient _llmClient = LlmClient.Create("openAi");

    public async Task<Result<Answer>> GenerateAnswer(Prompt prompt)
    {
        var messages = new List<ChatMessage>
        {
            new(Role.System, "You are programmer assistant, please answer correctly as you can."),
            new(Role.User, prompt.Value)
        };

        var chatRequest = new ChatRequest(messages, Model.GPT4o, maxTokens: 4096);

        var response = await client.ChatEndpoint.GetCompletionAsync(chatRequest);

        var choice = response.FirstChoice;

        if (choice is null)
            return Result.Failure<Answer>(OpenAiClientErrors.EmptyAnswer.Build());

        var answer = choice.Message.ToString();
        var model = response.Model;
        var promptTokens = response.Usage.PromptTokens;
        var completionTokens = response.Usage.CompletionTokens;

        return Audit.Create(model, promptTokens, completionTokens)
            .Bind(audit => Answer.Create(answer, prompt.Value, _llmClient, audit));
    }
    
}