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
        Console.WriteLine($"\nQuestion: '{question}'\n");
        
        var result = await knowledgeService.RecallAsync(question);
        
        Console.WriteLine($"\nAnswer: '{result.Value}'\n");
    }

    public async Task LearnAsync()
    {
        // var notesLearnResult = await fileParser
        //     .GetNotes()
        //     .Bind(async resources =>
        //     {
        //         var results = new List<Result>();
        //         
        //         foreach (var resource in resources)
        //         {
        //             var learnResult = await knowledgeService.LearnAsync(resource, ResourceType.Note);
        //             
        //             results.Add(learnResult);
        //         }
        //
        //         return Result.Combine(results);
        //     });
        
        var codeLearnResult = await fileParser
            .GetCode()
            .Bind(async resources =>
            {
                var results = new List<Result>();
                
                foreach (var resource in resources)
                {
                    var learnResult = await knowledgeService.LearnAsync(resource, ResourceType.Code);
                    
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