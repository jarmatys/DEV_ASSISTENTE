using ASSISTENTE.Infrastructure.Image.Contracts;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Errors;
using CSharpFunctionalExtensions;
using OpenAI;
using OpenAI.Images;

namespace ASSISTENTE.Infrastructure.Image;

internal sealed class OpenAiClient(OpenAIClient client) : IImageClient
{
    public async Task<Result<ImageDetails>> GenerateImage(ImagePrompt imagePrompt)
    {
        var request = new ImageGenerationRequest(
            prompt: imagePrompt.Prompt,
            size: "1024x1024"
        );

        var response = await client.ImagesEndPoint.GenerateImageAsync(request);
        
        if (response is null)
            return Result.Failure<ImageDetails>(ClientErrors.EmptyAnswer.Build());
        
        var imageDetails = response.FirstOrDefault();
        
        return ImageDetails.Create(imageDetails?.Url);
    }
}