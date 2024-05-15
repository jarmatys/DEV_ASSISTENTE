using ASSISTENTE.Application.Abstractions.ValueObjects;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Infrastructure.MarkDownParser.ValueObjects;

namespace ASSISTENTE.Infrastructure.Services.Parsers;

public sealed partial class FileParser 
{
    public Result<IEnumerable<ResourceText>> GetNotes()
    {
        var filePaths = GetPaths(knowledgePaths.MarkdownNotes);

        var resourceBlocks = new List<ResourceText>();
        foreach (var fileLocation in filePaths)
        {
            if (!fileLocation.EndsWith(".md")) continue;

            var blocks = FilePath.Create(fileLocation)
                .Bind(markDownParser.Parse)
                .Map(parsedFile => parsedFile.TextBlocks.Select(content => ResourceText.Create(parsedFile.Title, content)))
                .LogError(logger)
                .GetValueOrDefault();

            if (blocks != null)
                resourceBlocks.AddRange(blocks);
        }

        return resourceBlocks;
    }
}