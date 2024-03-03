using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Infrastructure.FileParser;
using ASSISTENTE.Infrastructure.FileParser.ValueObjects;

namespace ASSISTENTE.Client;

public sealed class Playground(IFileParser fileParser)
{
    public void Start()
    {
        Console.WriteLine("Starting Playground...");

        var content = FilePath.Create("test-file.md")
            .OnSuccess(path => fileParser.Parse(path))
            .OnFailure(Console.WriteLine);
        
        Console.WriteLine("Stopping Playground...");
    }
}