using ASSISTENTE.Common.Results;
using ASSISTENTE.Infrastructure.MarkDownParser.Models;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.MarkDownParser.ValueObjects;

public sealed class FileContent
{
    public IEnumerable<string> TextBlocks { get; }

    private FileContent(IEnumerable<string> textBlocks)
    {
        TextBlocks = textBlocks;
    }

    public static Result<FileContent> Create(List<ElementBase> elements)
    {
        if (elements.Count == 0)
            return Result.Failure<FileContent>(FileContentErrors.EmptyContent.Build());

        // TODO: Group by headings and prepare smaller text blocks
        var content = string.Join("\n", elements.Select(x => x.Content));

        return new FileContent([content]);
    }
}

public static class FileContentErrors
{
    public static readonly Error EmptyContent = new(
        "FileContent.EmptyContent", "File content is empty");
}