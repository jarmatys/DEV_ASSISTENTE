using ASSISTENTE.Infrastructure.LLM.OpenAi.Errors;
using ASSISTENTE.Infrastructure.Vision.Contracts;
using CSharpFunctionalExtensions;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using ChatMessage = OpenAI.Chat.Message;

namespace ASSISTENTE.Infrastructure.Vision;

internal sealed class OpenAiClient(OpenAIClient client) : IVisionClient
{
    public async Task<Result<Recognition>> Recognize(VisionImage visionImage)
    {
        var imageUrl = new ImageUrl(visionImage.ImageUrl);
        var messages = new List<ChatMessage>
        {
            new(Role.System, visionImage.Prompt),
            new(Role.User, new List<Content>
            {
                new(imageUrl)
            })
        };

        var chatRequest = new ChatRequest(messages, Model.GPT4o, maxTokens: 4096);

        var response = await client.ChatEndpoint.GetCompletionAsync(chatRequest);
        
        var recognitionText = response.FirstChoice;

        if (recognitionText is null)
            return Result.Failure<Recognition>(ClientErrors.EmptyAnswer.Build());
        
        return Recognition.Create(recognitionText);
    }
}