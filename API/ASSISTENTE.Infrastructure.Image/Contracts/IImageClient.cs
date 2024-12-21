using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Image.Contracts;

public interface IImageClient
{
    Task<Result<ImageDetails>> GenerateImage(ImagePrompt imagePrompt);
}