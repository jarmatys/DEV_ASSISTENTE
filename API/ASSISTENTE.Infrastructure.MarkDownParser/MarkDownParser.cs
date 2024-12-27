using ASSISTENTE.Infrastructure.MarkDownParser.Contracts;
using ASSISTENTE.Infrastructure.MarkDownParser.Contracts.Models;
using ASSISTENTE.Infrastructure.MarkDownParser.Errors;
using ASSISTENTE.Infrastructure.MarkDownParser.Extensions;
using ASSISTENTE.Infrastructure.MarkDownParser.Models;
using CSharpFunctionalExtensions;
using Markdig;
using Markdig.Syntax;

namespace ASSISTENTE.Infrastructure.MarkDownParser;

internal sealed class MarkDownParser : IMarkDownParser
{
    private readonly List<Type> _supportedBlocks =
    [
        typeof(HeadingBlock),
        typeof(ListBlock),
        typeof(CodeBlock),
        typeof(ParagraphBlock),
        typeof(FencedCodeBlock)
    ];

    // TODO: https://github.com/microsoft/markitdown - library parsing a lot of formats to markdown
    
    public Result<FileContent> Parse(FilePath filePath)
    {
        // TODO: calculate chunk tokens and split by 1000 tokens
        
        var content = File.ReadAllText(filePath.Path);
        var parsedMarkdown = Markdown.Parse(content);

        var elementResults = parsedMarkdown
            .Where(element => _supportedBlocks.Contains(element.GetType()))
            .Select(GetElement)
            .ToList();

        if (elementResults.Count == 0)
            return Result.Failure<FileContent>(MarkDownParserErrors.EmptyContent.Build());

        if (elementResults.Any(x => x.IsFailure))
            return Result.Failure<FileContent>(MarkDownParserErrors.UnsupportedBlock.Build());

        var elements = elementResults
            .Select(x => x.Value)
            .ToList();

        if (elements.All(x => x is Heading))
            return Result.Failure<FileContent>(MarkDownParserErrors.OnlyHeadersNotAllowed.Build());

        return FileContent.Create(filePath.FileName, elements);
    }

    public Result<List<MediaUrl>> GetMediaUrls(FilePath filePath)
    {
        var content = File.ReadAllText(filePath.Path);
        var parsedMarkdown = Markdown.Parse(content);

        var mediaUrls = parsedMarkdown
            .Descendants<ParagraphBlock>()
            .Select(paragraphBlock => paragraphBlock.GetUrls())
            .Where(mediaUrl => mediaUrl.Count != 0)
            .SelectMany(mediaUrl => mediaUrl)
            .Select(MediaUrl.Create)
            .ToList();
        
        if (mediaUrls.Count == 0)
            return Result.Failure<List<MediaUrl>>(MarkDownParserErrors.EmptyContent.Build());
        
        if (mediaUrls.Any(x => x.IsFailure))
            return Result.Failure<List<MediaUrl>>(MarkDownParserErrors.UnsupportedMedia.Build());
        
        return mediaUrls
            .Select(x => x.Value)
            .ToList();
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
            case FencedCodeBlock code:
            {
                var codeContent = code.GetCodeBlock();

                var codeElement = new Code(codeContent) as ElementBase;

                return Result.Success(codeElement);
            }
            case CodeBlock code:
            {
                var codeContent = code.GetCodeLine();

                var codeElement = new Code(codeContent) as ElementBase;

                return Result.Success(codeElement);
            }
            case ParagraphBlock paragraph:
            {
                var paragraphContent = paragraph.GetContent();
                var paragraphUrls = paragraph.GetUrls();

                var paragraphElement = new Paragraph(paragraphContent, paragraphUrls) as ElementBase;

                return Result.Success(paragraphElement);
            }
            default:
                return Result.Failure<ElementBase>(MarkDownParserErrors.UnsupportedBlock.Build());
        }
    }
}