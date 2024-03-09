using ASSISTENTE.Infrastructure.CodeParser;
using ASSISTENTE.Infrastructure.CodeParser.ValueObjects;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Embeddings.ValueObjects;
using ASSISTENTE.Infrastructure.FileParser;
using ASSISTENTE.Infrastructure.FileParser.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Client;

public sealed class Playground(IFileParser fileParser, ICodeParser codeParser, IEmbeddingClient embeddingClient)
{
    public void Start()
    {
        Console.WriteLine("Starting Playground...");

        var mdFileContent = FilePath.Create("Examples/test-file.md")
            .Bind(fileParser.Parse)
            .Map(fileContent => fileContent.Content)
            .TapError(errors => Console.WriteLine(errors))
            .GetValueOrDefault();

        var codeContent = CodeFile.Create("Examples/test-file.cs")
            .Bind(codeParser.Parse)
            .Map(fileContent => fileContent.Classes)
            .TapError(errors => Console.WriteLine(errors))
            .GetValueOrDefault();
        
        var embeddings = EmbeddingText.Create(mdFileContent)
            .Bind(async text => await embeddingClient.GetAsync(text))
            .Map(embedding => embedding)
            .TapError(errors => Console.WriteLine(errors));
        
        Console.WriteLine("Stopping Playground...");
    }
}