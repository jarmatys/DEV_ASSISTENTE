using ASSISTENTE.Domain.Enums;
using ASSISTENTE.Infrastructure.Interfaces;

namespace ASSISTENTE.Client;

public sealed class Playground(
    IFileParser fileParser, 
    IKnowledgeService knowledgeService, 
    IMaintenanceService maintenanceService)
{
    public async Task AnswerAsync(string question)
    {
        var result = await knowledgeService.RecallAsync(question);
    }

    public async Task LearnAsync()
    {
        var notes = fileParser.Parse("Examples/test-notes.md")
            .GetValueOrDefault();
        
        foreach (var note in notes)
        {
            var knowledge = await knowledgeService.LearnAsync(note, ResourceType.Note);
        }
    }
    
    public async Task ResetAsync()
    {
        await maintenanceService.ResetAsync();
        await maintenanceService.InitAsync();
    }
}