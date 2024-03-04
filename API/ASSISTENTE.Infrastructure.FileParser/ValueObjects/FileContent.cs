using ASSISTENTE.Common;
using ASSISTENTE.Common.Extensions;

namespace ASSISTENTE.Infrastructure.FileParser.ValueObjects;

public sealed class FileContent
{
    public string Content { get; }

    private FileContent(string content)
    {
        Content = content;
    }
    
    public static Result<FileContent> Create(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return Result<FileContent>.Fail(FileContentErrors.EmptyContent);
        
        return new FileContent(content).ToResult();
    }
}

public static class FileContentErrors
{
    public static readonly Error EmptyContent = new(
        "FileContent.EmptyContent", "File content is empty");
}