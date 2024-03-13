using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Interfaces;

public interface IFileParser
{
    public Result<List<string>> Parse(string filePath);
}