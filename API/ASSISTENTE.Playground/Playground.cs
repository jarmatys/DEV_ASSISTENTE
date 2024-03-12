using ASSISTENTE.Infrastructure.CodeParser;
using ASSISTENTE.Infrastructure.CodeParser.ValueObjects;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Embeddings.ValueObjects;
using ASSISTENTE.Infrastructure.FileParser;
using ASSISTENTE.Infrastructure.FileParser.ValueObjects;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Qdrant.Models;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Client;

public sealed class Playground(
    IFileParser fileParser, 
    ICodeParser codeParser, 
    IEmbeddingClient embeddingClient,
    IQdrantService qdrantService)
{
    public async Task StartAsync()
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

        var embeddings = await EmbeddingText.Create(mdFileContent)
            .Bind(embeddingClient.GetAsync)
            .Map(embedding => embedding)
            .TapError(errors => Console.WriteLine(errors));
        
        await qdrantService.CreateCollectionAsync("embeddings");
        
        var upsertResult = await DocumentDto.Create(embeddings.Value.Embeddings, "embeddings")
            .Bind(qdrantService.UpsertAsync)
            .TapError(errors => Console.WriteLine(errors));
        
        Console.WriteLine("Stopping Playground...");
    }
}