using CSharpFunctionalExtensions;
using ASSISTENTE.Infrastructure.FileParser.Models;

namespace ASSISTENTE.Infrastructure.FileParser.ValueObjects;

public sealed class FileContent
{
    public string Content { get; }

    private FileContent(string content)
    {
        Content = content;
    }
    
    public static Result<FileContent> Create(List<ElementBase> elements)
    {
        if (elements.Count == 0)
            return Result<FileContent>.Fail(FileContentErrors.EmptyContent);
        
        var content = string.Join("\n\n", elements.Select(x => x.Content));
        
        return new FileContent(content).ToResult();
    }
}

public static class FileContentErrors
{
    public static readonly Error EmptyContent = new(
        "FileContent.EmptyContent", "File content is empty");
}