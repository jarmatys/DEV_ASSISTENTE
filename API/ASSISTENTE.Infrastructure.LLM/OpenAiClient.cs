using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Errors;
using CSharpFunctionalExtensions;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Files;
using OpenAI.FineTuning;
using OpenAI.Models;
using ChatMessage = OpenAI.Chat.Message;

namespace ASSISTENTE.Infrastructure.LLM;

internal sealed class OpenAiClient(OpenAIClient client) : ILlmClient
{
    private readonly LlmClient _llmClient = LlmClient.Create("openAi");

    public async Task<Result<Answer>> GenerateAnswer(Prompt prompt)
    {
        var systemPrompt = prompt.System ?? "You are programmer assistant, please answer correctly as you can.";

        var messages = new List<ChatMessage>
        {
            new(Role.System, systemPrompt),
            new(Role.User, prompt.Value)
        };

        var chosenModel = prompt.Model ?? Model.GPT4o;

        var chatRequest = new ChatRequest(messages, chosenModel, maxTokens: 4096);

        var response = await client.ChatEndpoint.GetCompletionAsync(chatRequest);

        var choice = response.FirstChoice;

        if (choice is null)
            return Result.Failure<Answer>(ClientErrors.EmptyAnswer.Build());

        var answer = choice.Message.ToString();
        var model = response.Model;
        var promptTokens = response.Usage.PromptTokens;
        var completionTokens = response.Usage.CompletionTokens;

        return Audit.Create(model, promptTokens, completionTokens)
            .Bind(audit => Answer.Create(answer, prompt.Value, _llmClient, audit));
    }

    public async Task<Result> FineTune(FineTuning fineTuning)
    {
        var fileUploadRequest = new FileUploadRequest(
            filePath: fineTuning.FilePath,
            purpose: "fine-tune"
        );
        
        var fileId = await client.FilesEndpoint.UploadFileAsync(fileUploadRequest);
            
        var fineTuningRequest = new CreateFineTuneJobRequest(
            model: Model.GPT4o,
            trainingFileId: fileId
        );

        var response = await client.FineTuningEndpoint.CreateJobAsync(fineTuningRequest);

        return Result.Success();
    }
}