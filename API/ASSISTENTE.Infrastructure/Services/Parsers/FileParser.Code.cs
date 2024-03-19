using ASSISTENTE.Infrastructure.CodeParser.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Services.Parsers;

public sealed partial class FileParser 
{
    public Result<IEnumerable<string>> GetCode()
    {
        var filePaths = GetPaths(knowledgePathsOption.Repositories);

        var blocks = new List<string>();
        foreach (var fileLocation in filePaths)
        {
            if (!fileLocation.EndsWith(".cs")) continue;
            
            var codeBlocks = CodeFile.Create(fileLocation)
                .Bind(codeParser.Parse)
                .Map(fileContent => fileContent.Classes)
                .GetValueOrDefault();
            
            if (codeBlocks != null)
                blocks.AddRange(codeBlocks);
        }

        return blocks;
    }
}