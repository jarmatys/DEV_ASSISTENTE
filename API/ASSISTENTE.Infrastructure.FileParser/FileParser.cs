using CSharpFunctionalExtensions;
using ASSISTENTE.Infrastructure.FileParser.Errors;
using ASSISTENTE.Infrastructure.FileParser.Extensions;
using ASSISTENTE.Infrastructure.FileParser.Models;
using ASSISTENTE.Infrastructure.FileParser.ValueObjects;
using Markdig;
using Markdig.Syntax;

namespace ASSISTENTE.Infrastructure.FileParser;

internal sealed class FileParser : IFileParser
{
    private readonly List<Type> _supportedBlocks =
    [
        typeof(HeadingBlock),
        typeof(ListBlock),
        typeof(CodeBlock),
        typeof(ParagraphBlock)
    ];

    public Result<FileContent> Parse(FilePath filePath)
    {
        var content = File.ReadAllText(filePath.Path);
        var parsedMarkdown = Markdown.Parse(content);

        var elementResults = parsedMarkdown
            .Where(element => _supportedBlocks.Contains(element.GetType()))
            .Select(GetElement)
            .ToList();

        if (elementResults.Count == 0)
            return Result.Failure<FileContent>(FileParserErrors.EmptyContent.Build());

        if (elementResults.Any(x => x.IsFailure))
            return Result.Failure<FileContent>(FileParserErrors.UnsupportedBlock.Build());

        var elements = elementResults
            .Select(x => x.Value)
            .ToList();

        return FileContent.Create(elements!)
            .TapError(error => Result.Failure<FileContent>(error));
    }

    private static Result<ElementBase> GetElement(IMarkdownObject block)
    {
        switch (block)
        {
            case HeadingBlock heading:
            {
                var level = heading.Level;

                var headingContent = heading.GetContent();

                var headingElement = new Heading(headingContent, level) as ElementBase;
                
                return Result.Success(headingElement);
            }
            case ListBlock list:
            {
                var listContent = list.GetListContent();

                var listElement = new NumberedList(listContent) as ElementBase;

                return Result.Success(listElement);
            }
            case CodeBlock code:
            {
                var codeContent = code.GetCode();
                
                var codeElement = new Code(codeContent) as ElementBase;

                return Result.Success(codeElement);
            }
            case ParagraphBlock paragraph:
            {
                var paragraphContent = paragraph.GetContent();

                var paragraphElement = new Paragraph(paragraphContent) as ElementBase;
                
                return Result.Success(paragraphElement);
            }
            default:
                return Result.Failure<ElementBase>(FileParserErrors.UnsupportedBlock.Build());
        }
    }
}