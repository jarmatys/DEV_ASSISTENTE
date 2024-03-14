using ASSISTENTE.Infrastructure.Interfaces;

namespace ASSISTENTE.Client;

public sealed class Playground(IFileParser fileParser, IKnowledgeService knowledgeService)
{
    public async Task StartAsync()
    {
        Console.WriteLine("Starting Playground...");

        var content = fileParser.Parse("Example/test-file.md")
            .GetValueOrDefault();
        
        var knowledge = await knowledgeService.LearnAsync("");
        
        const string query = "How to create a new file in C#?";
        
        var result = await knowledgeService.RecallAsync(query);
        
        Console.WriteLine("Stopping Playground...");
    }
}