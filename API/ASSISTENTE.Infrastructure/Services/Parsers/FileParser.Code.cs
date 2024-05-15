using ASSISTENTE.Application.Abstractions.ValueObjects;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Infrastructure.CodeParser.ValueObjects;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Infrastructure.Services.Parsers;

public sealed partial class FileParser
{
    private readonly string _repositoriesPath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "Resources",
        "Repositories"
    );
    
    public Result<IEnumerable<ResourceText>> GetCode()
    {
        var filePaths = GetPaths(_repositoriesPath)!;

        if (filePaths.Count == 0)
            logger.LogWarning("No code files found in the path '{Path}'. Mount location as volume.", _repositoriesPath);
        
        var resourceBlocks = new List<ResourceText>();
        foreach (var fileLocation in filePaths)
        {
            if (!fileLocation.EndsWith(".cs")) continue;
            
            var blocks = CodePath.Create(fileLocation)
                .Bind(codeParser.Parse)
                .Map(parsedFile => parsedFile.CodeBlocks.Select(content => ResourceText.Create(parsedFile.Title, content)))
                .LogError(logger)
                .GetValueOrDefault();
            
            if (blocks != null)
                resourceBlocks.AddRange(blocks);
        }

        return resourceBlocks;
    }
}