using ASSISTENTE.Infrastructure.MarkDownParser.ValueObjects;
using ASSISTENTE.Infrastructure.ValueObjects;

namespace ASSISTENTE.Infrastructure.Services.Parsers;

public sealed partial class FileParser 
{
    public Result<IEnumerable<ResourceText>> GetNotes()
    {
        var filePaths = GetPaths(knowledgePathsOption.MarkdownNotes);

        var resourceBlocks = new List<ResourceText>();
        foreach (var fileLocation in filePaths)
        {
            if (!fileLocation.EndsWith(".md")) continue;

            var blocks = FilePath.Create(fileLocation)
                .Bind(markDownParser.Parse)
                .Map(parsedFile => parsedFile.TextBlocks.Select(content => ResourceText.Create(parsedFile.Title, content)))
                .TapError(error => Console.WriteLine(error))
                .GetValueOrDefault();

            if (blocks != null)
                resourceBlocks.AddRange(blocks);
        }

        return resourceBlocks;
    }
}