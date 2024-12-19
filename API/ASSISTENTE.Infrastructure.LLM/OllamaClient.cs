using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.LLM.Ollama.Errors;
using ASSISTENTE.Infrastructure.LLM.Ollama.Settings;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using OllamaSharp;
using OllamaSharp.Models;

namespace ASSISTENTE.Infrastructure.LLM;

internal sealed class OllamaClient(OllamaApiClient client, IOptions<OllamaSettings> options) : ILlmClient
{
    private readonly LlmClient _llmClient = LlmClient.Create("ollama");

    public async Task<Result<Answer>> GenerateAnswer(Prompt prompt)
    {
        var request = new GenerateRequest
        {
            Stream = false,
            System = "Always answer short and correct",
            Prompt = prompt.Value,
            Model = options.Value.SelectedModel
        };

        string? answer = null;
        string? model = null;
        
        await foreach (var answerToken in client.GenerateAsync(request))
        {
            answer += answerToken?.Response;
            model = answerToken?.Model;
        }

        if (answer is null)
            return Result.Failure<Answer>(ClientErrors.EmptyAnswer.Build());

        return Audit.Create(
                model: model,
                promptTokens: 0,
                completionTokens: 0
            )
            .Bind(audit => Answer.Create(answer, prompt.Value, _llmClient, audit));
    }
}