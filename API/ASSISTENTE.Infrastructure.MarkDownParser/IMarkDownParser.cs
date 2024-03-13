using ASSISTENTE.Infrastructure.MarkDownParser.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.MarkDownParser;

public interface IMarkDownParser
{
    public Result<FileContent> Parse(FilePath filePath);
}