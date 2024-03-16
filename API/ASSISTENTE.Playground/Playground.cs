using ASSISTENTE.Domain.Enums;
using ASSISTENTE.Infrastructure.Interfaces;

namespace ASSISTENTE.Client;

public sealed class Playground(
    IFileParser fileParser, 
    IKnowledgeService knowledgeService, 
    IMaintenanceService maintenanceService)
{
    public async Task StartAsync()
    {
        Console.WriteLine("Starting Playground...");
        
        var notes = fileParser.Parse("Examples/test-notes.md")
            .GetValueOrDefault();

        foreach (var note in notes)
        {
            var knowledge = await knowledgeService.LearnAsync(note, ResourceType.Note);
        }
        
        //const string query = "What does Adam Kowalski do professionally?";
        
        //var result = await knowledgeService.RecallAsync(query);
        
        Console.WriteLine("Stopping Playground...");
    }

    public async Task CleanAsync()
    {
        Console.WriteLine("Start cleaning...");

        await maintenanceService.ResetAsync();
        
        Console.WriteLine("Stop cleaning...");
    }
    
    public async Task InitAsync()
    {
        Console.WriteLine("Start initialization...");

        await maintenanceService.InitAsync();
        
        Console.WriteLine("Stop initialization...");
    }
}