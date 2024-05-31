using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.MarkDownParser.Contracts;

public interface IMarkDownParser
{
    public Result<FileContent> Parse(FilePath filePath);
}