using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.MarkDownParser.Contracts;

public interface IMarkDownParser
{
    public Result<FileContent> Parse(FilePath filePath);
    public Result<List<MediaUrl>> GetMediaUrls(FilePath filePath);
}