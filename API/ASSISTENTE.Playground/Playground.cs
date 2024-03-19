using ASSISTENTE.Domain.Enums;
using ASSISTENTE.Infrastructure.Interfaces;
using CSharpFunctionalExtensions;

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
        var notesLearnResult = await fileParser
            .GetNotes()
            .Bind(async texts =>
            {
                var results = new List<Result>();
                
                foreach (var text in texts)
                {
                    var learnResult = await knowledgeService.LearnAsync(text, ResourceType.Note);
                    
                    results.Add(learnResult);
                }

                return Result.Combine(results);
            });
        
        var codeLearnResult = await fileParser
            .GetCode()
            .Bind(async texts =>
            {
                var results = new List<Result>();
                
                foreach (var text in texts)
                {
                    var learnResult = await knowledgeService.LearnAsync(text, ResourceType.Code);
                    
                    results.Add(learnResult);
                }

                return Result.Combine(results);
            });
    }

    public async Task ResetAsync()
    {
        await maintenanceService.ResetAsync();
        await maintenanceService.InitAsync();
    }
}