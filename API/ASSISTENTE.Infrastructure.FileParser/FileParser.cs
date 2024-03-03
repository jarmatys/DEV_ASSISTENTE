using ASSISTENTE.Common;
using ASSISTENTE.Infrastructure.FileParser.ValueObjects;

namespace ASSISTENTE.Infrastructure.FileParser;

internal sealed class FileParser : IFileParser
{
    public Result<FileContent> Parse(FilePath filePath)
    {
        var content = File.ReadAllText(filePath.Path);
        
        return Result<FileContent>.Ok();
    }
}