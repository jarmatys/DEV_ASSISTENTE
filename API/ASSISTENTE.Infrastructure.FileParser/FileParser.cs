using ASSISTENTE.Common;
using ASSISTENTE.Common.Extensions;
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
            return Result<FileContent>.Fail(FileParserErrors.EmptyContent);
        
        if (elementResults.Any(x => x.IsFailure))
            return Result<FileContent>.Fail(FileParserErrors.UnsupportedBlock);
        
        var elements = elementResults
            .Select(x => x.Value)
            .ToList();
        
        return FileContent.Create(elements!)
            .OnFailure(error => Result<FileContent>.Fail(error));
    }
    
    private static Result<ElementBase> GetElement(IMarkdownObject block)
    {
        switch (block)
        {
            case HeadingBlock heading:
            {
                var level = heading.Level;
                
                var headingContent = heading.GetContent();
                
                return Result<ElementBase>.Ok(new Heading(headingContent, level));
            }
            case ListBlock list:
            {
                var listContent = list.GetListContent();
                
                return Result<ElementBase>.Ok(new NumberedList(listContent));
            }
            case CodeBlock code:
            {
                var codeContent = code.GetCode();
                
                return Result<ElementBase>.Ok(new Code(codeContent));
            }
            case ParagraphBlock paragraph:
            {
                var paragraphContent = paragraph.GetContent();
                
                return Result<ElementBase>.Ok(new Paragraph(paragraphContent));
            }
            default:
                return Result<ElementBase>.Fail(FileParserErrors.UnsupportedBlock);
        }
    }
}