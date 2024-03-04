using ASSISTENTE.Common;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Infrastructure.FileParser.Extensions;
using ASSISTENTE.Infrastructure.FileParser.ValueObjects;
using Markdig;
using Markdig.Syntax;

namespace ASSISTENTE.Infrastructure.FileParser;

internal sealed class FileParser : IFileParser
{
    public Result<FileContent> Parse(FilePath filePath)
    {
        var content = File.ReadAllText(filePath.Path);

        var fileContent = FileContent.Create(content)
            .OnFailure(error => Result<FileContent>.Fail(error));

        var parsedMarkdown = Markdown.Parse(content);

        foreach (var item in parsedMarkdown.Descendants())
        {
            if (item is HeadingBlock heading)
            {
                var headingContent = heading.GetContent();

                Console.WriteLine(headingContent);

                Console.WriteLine("----\n");
            }

            if (item is ParagraphBlock paragraph)
            {
                var paragraphContent = paragraph.GetContent();
                
                Console.WriteLine(paragraphContent);
                
                Console.WriteLine("----\n");
            }
        }

        return fileContent;
    }
}