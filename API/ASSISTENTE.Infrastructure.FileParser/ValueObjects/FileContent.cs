using ASSISTENTE.Common.Results;
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
            return Result.Failure<FileContent>(FileContentErrors.EmptyContent.Build());
        
        // TODO: Group by heading level
        var content = string.Join("\n\n", elements.Select(x => x.Content));

        return new FileContent(content);
    }
}

public static class FileContentErrors
{
    public static readonly Error EmptyContent = new(
        "FileContent.EmptyContent", "File content is empty");
}