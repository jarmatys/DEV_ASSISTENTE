using ASSISTENTE.Infrastructure.MarkDownParser.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Services.Parsers;

public sealed partial class FileParser 
{
    public Result<IEnumerable<string>> GetNotes()
    {
        var filePaths = GetPaths(knowledgePathsOption.MarkdownNotes);

        var blocks = new List<string>();
        foreach (var fileLocation in filePaths)
        {
            if (!fileLocation.EndsWith(".md")) continue;

            var textBlocks = FilePath.Create(fileLocation)
                .Bind(markDownParser.Parse)
                .Map(fileContent => fileContent.TextBlocks)
                .TapError(error => Console.WriteLine(error))
                .GetValueOrDefault();

            if (textBlocks != null)
                blocks.AddRange(textBlocks);
        }

        return blocks;
    }
}