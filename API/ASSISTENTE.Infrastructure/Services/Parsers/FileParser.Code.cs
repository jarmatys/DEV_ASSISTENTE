using ASSISTENTE.Infrastructure.CodeParser.ValueObjects;
using ASSISTENTE.Infrastructure.ValueObjects;

namespace ASSISTENTE.Infrastructure.Services.Parsers;

public sealed partial class FileParser 
{
    public Result<IEnumerable<ResourceText>> GetCode()
    {
        var filePaths = GetPaths(knowledgePathsOption.Repositories);

        var resourceBlocks = new List<ResourceText>();
        foreach (var fileLocation in filePaths)
        {
            if (!fileLocation.EndsWith(".cs")) continue;
            
            var blocks = CodePath.Create(fileLocation)
                .Bind(codeParser.Parse)
                .Map(parsedFile => parsedFile.CodeBlocks.Select(content => ResourceText.Create(parsedFile.Title, content)))
                .TapError(error => Console.WriteLine(error))
                .GetValueOrDefault();
            
            if (blocks != null)
                resourceBlocks.AddRange(blocks);
        }

        return resourceBlocks;
    }
}