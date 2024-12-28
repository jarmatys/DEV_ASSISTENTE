using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Pdf4Me.Contracts;

public interface IPdf4MeService
{
    Task<Result<ExtractData>> ExtractDataAsync(string filePath);
}