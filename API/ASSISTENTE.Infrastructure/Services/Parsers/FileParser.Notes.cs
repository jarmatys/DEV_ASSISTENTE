using ASSISTENTE.Application.Abstractions.ValueObjects;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Infrastructure.MarkDownParser.Contracts;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Infrastructure.Services.Parsers;

public sealed partial class FileParser
{
    private readonly string _notesPath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "Resources",
        "Notes"
    );
    
    public Result<IEnumerable<ResourceText>> GetNotes()
    {
        var filePaths = GetPaths(_notesPath)!;

        if (filePaths.Count == 0)
            logger.LogWarning("No notes files found in the path '{Path}'. Mount location as volume.", _notesPath);

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