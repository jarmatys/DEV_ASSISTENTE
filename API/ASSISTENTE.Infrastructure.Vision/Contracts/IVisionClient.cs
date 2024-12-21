using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Vision.Contracts;

public interface IVisionClient
{
    Task<Result<Recognition>> Recognize(VisionImage visionImage);
}