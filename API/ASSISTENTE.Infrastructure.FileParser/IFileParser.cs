using ASSISTENTE.Common;
using ASSISTENTE.Infrastructure.FileParser.ValueObjects;

namespace ASSISTENTE.Infrastructure.FileParser;

public interface IFileParser
{
    public Result<FileContent> Parse(FilePath filePath);
}