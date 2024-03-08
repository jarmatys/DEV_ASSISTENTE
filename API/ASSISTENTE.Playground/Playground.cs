using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Infrastructure.CodeParser;
using ASSISTENTE.Infrastructure.CodeParser.ValueObjects;
using ASSISTENTE.Infrastructure.FileParser;
using ASSISTENTE.Infrastructure.FileParser.ValueObjects;

namespace ASSISTENTE.Client;

public sealed class Playground(IFileParser fileParser, ICodeParser codeParser)
{
    public void Start()
    {
        Console.WriteLine("Starting Playground...");

        var mdFileContent = FilePath.Create("Examples/test-file.md")
            .OnSuccess(fileParser.Parse)
            .OnSuccess(fileContent => fileContent.Content)
            .OnFailure(Console.WriteLine);
        
        var codeContent = CodeFile.Create("Examples/test-file.cs")
            .OnSuccess(codeParser.Parse)
            .OnSuccess(fileContent => fileContent.Classes)
            .OnFailure(Console.WriteLine);
        
        Console.WriteLine("Stopping Playground...");
    }
}